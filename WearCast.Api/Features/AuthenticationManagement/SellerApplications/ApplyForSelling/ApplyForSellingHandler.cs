
using System.Security.Cryptography;
using WearCast.Api.Common.Email;
using WearCast.Api.Common.Services;

namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ApplyForSelling
{
    public class ApplyForSellingHandler(
        ApplicationDbContext context,
        IMapper mapper,
        EmailHelper emailHelper,
        ImageService imageService,
        IPasswordHasher<SellerApplication> passwordHasher)
        : IRequestHandler<ApplyForSellingRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly EmailHelper _emailHelper = emailHelper;
        private readonly ImageService _imageService = imageService;
        private readonly IPasswordHasher<SellerApplication> _passwordHasher = passwordHasher;

        public async Task<Result> Handle(ApplyForSellingRequest request, CancellationToken cancellationToken)
        {
            var identityConflict = await _context.Users
                .Where(u => u.Email == request.Email || u.PhoneNumber == request.PhoneNumber)
                .Select(u => new { u.Email, u.PhoneNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (identityConflict != null)
            {
                if (identityConflict.Email == request.Email)
                    return Result.Failure(SellerApplicationErrors.EmailInUse);

                return Result.Failure(SellerApplicationErrors.PhoneInUse);
            }

            var phoneConflictInApps = await _context.SellerApplications
                .AnyAsync(x => x.PhoneNumber == request.PhoneNumber
                            && x.Email != request.Email
                            && x.Status != Status.Rejected, cancellationToken);

            if (phoneConflictInApps)
                return Result.Failure(SellerApplicationErrors.PhoneInUse);

            var logoResult = await ValidateAndUploadLogoAsync(request.Logo);
            if (logoResult.IsFailure)
                return Result.Failure(logoResult.Error);

            var existingApp = await _context.SellerApplications
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (existingApp != null)
            {
                if (existingApp.Status == Status.Pending)
                    return Result.Failure(SellerApplicationErrors.ApplicationPending);

                if (existingApp.Status == Status.Approved)
                    return Result.Failure(SellerApplicationErrors.SellerAlreadyExists);

                if (existingApp.Status == Status.Rejected)
                {
                    _mapper.Map(request, existingApp);
                    existingApp.Status = Status.Pending;
                    existingApp.RejectionReason = null;
                    existingApp.LogoUrl = logoResult.Value;
                    existingApp.PasswordHash = _passwordHasher.HashPassword(existingApp, request.Password);

                    await GenerateAndSendConfirmationCodeAsync(existingApp);

                    _context.SellerApplications.Update(existingApp);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success();
                }
            }

            var newApp = _mapper.Map<SellerApplication>(request);

            newApp.LogoUrl = logoResult.Value;
            newApp.PasswordHash = _passwordHasher.HashPassword(newApp, request.Password);

            await GenerateAndSendConfirmationCodeAsync(newApp);

            newApp.CreatedOn = DateTime.Now;

            await _context.SellerApplications.AddAsync(newApp, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
        private async Task GenerateAndSendConfirmationCodeAsync(SellerApplication application)
        {
            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            application.EmailConfirmationCode = code;
            application.EmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);

            await _emailHelper.SendConfirmationEmailForSellerApplication(application, code);
        }
        private async Task<Result<string>> ValidateAndUploadLogoAsync(IFormFile logo)
        {
            var imageValidation = _imageService.Validate(logo);
            if (!imageValidation.IsValid)
            {
                return Result.Failure<string>(new Error("Image.Invalid", imageValidation.ErrorMessage, StatusCodes.Status400BadRequest));
            }

            var uploadedLogoUrl = await _imageService.UploadAsync(logo);
            return Result.Success(uploadedLogoUrl);
        }
    }
}
