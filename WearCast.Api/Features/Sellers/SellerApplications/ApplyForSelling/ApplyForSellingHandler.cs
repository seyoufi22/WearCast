using System.Security.Cryptography;

namespace WearCast.Api.Features.Sellers.SellerApplications.ApplyForSelling
{
    public class ApplyForSellingHandler(
        ApplicationDbContext context,
        IMapper mapper,
        EmailHelper emailHelper,
        ImageService imageService,
        IPasswordHasher<SellerApplication> passwordHasher,
        IMediator mediator)
        : IRequestHandler<ApplyForSellingRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly EmailHelper _emailHelper = emailHelper;
        private readonly ImageService _imageService = imageService;
        private readonly IPasswordHasher<SellerApplication> _passwordHasher = passwordHasher;
        private readonly IMediator _mediator = mediator;

        public async Task<Result> Handle(ApplyForSellingRequest request, CancellationToken cancellationToken)
        {
            var userConflict = await _context.Users
               .Where(u => u.Email == request.SellerManagerEmail || u.PhoneNumber == request.SellerManagerPhoneNumber)
               .Select(u => new { u.Email, u.PhoneNumber })
               .FirstOrDefaultAsync(cancellationToken);

            if (userConflict != null)
            {
                if (userConflict.Email.Equals(request.SellerManagerEmail?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure(SellerManagerErrors.EmailInUse);

                return Result.Failure(SellerManagerErrors.PhoneInUse);
            }

            var existingApp = await _context.SellerApplications
                .Where(x =>
                    x.Status != Status.Rejected &&
                    (
                    x.ManagerEmail == request.SellerManagerEmail ||
                    x.ManagerPhoneNumber == request.SellerManagerPhoneNumber ||
                    x.SellerName == request.SellerName ||
                    x.SellerEmail == request.SellerEmail ||
                    x.SellerPhoneNumber == request.SellerPhoneNumber ||
                    x.CommercialRegisterNumber == request.SellerCommercialRegisterNumber ||
                    x.TaxIdNumber == request.SellerTaxIdNumber
                    ))
                .FirstOrDefaultAsync(cancellationToken);

            if (existingApp != null)
            {
                if (existingApp.ManagerEmail.Equals(request.SellerManagerEmail?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure(SellerApplicationErrors.ApplicationPending);

                if (existingApp.ManagerPhoneNumber == request.SellerManagerPhoneNumber?.Trim())
                    return Result.Failure(SellerManagerErrors.PhoneInUse);

                if (existingApp.SellerEmail.Equals(request.SellerEmail?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure(SellerErrors.EmailInUse);

                if (existingApp.SellerPhoneNumber == request.SellerPhoneNumber?.Trim())
                    return Result.Failure(SellerErrors.PhoneInUse);

                if (existingApp.SellerName.Equals(request.SellerName?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure(SellerErrors.NameInUse);

                if (existingApp.CommercialRegisterNumber.Equals(request.SellerCommercialRegisterNumber?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure(SellerErrors.CommercialRegisterInUse);

                return Result.Failure(SellerErrors.TaxIdInUse);
            }


            var existingRejectedApp = await _context.SellerApplications
                .Where(x => x.ManagerEmail == request.SellerManagerEmail && x.Status == Status.Rejected)
                .FirstOrDefaultAsync(cancellationToken);

            var adminRoles = new[] { DefaultRoles.SuperAdmin, DefaultRoles.VendorAdmin };

            var roleIds = await _context.Roles
                .Where(r => adminRoles.Contains(r.Name))
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);

            var adminIds = await _context.UserRoles
                .Where(ur => roleIds.Contains(ur.RoleId))
                .Select(ur => ur.UserId.ToString())
                .Distinct()
                .ToListAsync(cancellationToken);

            if (existingRejectedApp != null)
            {
                var oldLogoUrl = existingRejectedApp.LogoUrl;

                _mapper.Map(request, existingRejectedApp);

                existingRejectedApp.Status = Status.Pending;
                existingRejectedApp.RejectionReason = null;
                existingRejectedApp.LogoUrl = await _imageService.UploadAsync(request.SellerLogo);
                existingRejectedApp.ManagerPasswordHash = _passwordHasher.HashPassword(existingRejectedApp, request.SellerManagerPassword);

                var confirmCode = GenerateConfirmationCode(existingRejectedApp);
                existingRejectedApp.CreatedOn = DateTime.UtcNow;

                _context.SellerApplications.Update(existingRejectedApp);
                await _context.SaveChangesAsync(cancellationToken);

                if (!string.IsNullOrEmpty(oldLogoUrl))
                    await _imageService.DeleteAsync(oldLogoUrl);

                await _emailHelper.SendConfirmationEmailForSellerManager(existingRejectedApp, confirmCode);

                if (adminIds.Any())
                {
                    var notificationEvent = new NewSellerApplicationEvent(
                        RecipientIds: adminIds,
                        ApplicationId: existingRejectedApp.Id,
                        SellerName: request.SellerName
                    );
                    await _mediator.Publish(notificationEvent, cancellationToken);
                }

                return Result.Success();
            }

            var newApp = _mapper.Map<SellerApplication>(request);

            newApp.LogoUrl = await _imageService.UploadAsync(request.SellerLogo);
            newApp.ManagerPasswordHash = _passwordHasher.HashPassword(newApp, request.SellerManagerPassword);
            newApp.CreatedOn = DateTime.UtcNow;

            var confirmationCode = GenerateConfirmationCode(newApp);

            await _context.SellerApplications.AddAsync(newApp, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailHelper.SendConfirmationEmailForSellerManager(newApp, confirmationCode);

            if (adminIds.Any())
            {
                var notificationEvent = new NewSellerApplicationEvent(
                    RecipientIds: adminIds,
                    ApplicationId: newApp.Id,
                    SellerName: request.SellerName
                );
                await _mediator.Publish(notificationEvent, cancellationToken);
            }

            return Result.Success();
        }

        private string GenerateConfirmationCode(SellerApplication application)
        {
            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            application.ManagerEmailConfirmationCode = code;
            application.ManagerEmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);
            application.ManagerEmailConfirmed = false;

            return code;
        }
    }
}