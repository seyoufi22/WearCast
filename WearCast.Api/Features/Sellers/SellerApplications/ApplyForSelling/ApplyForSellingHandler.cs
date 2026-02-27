using System.Security.Cryptography;




// تأكد من استدعاء مسارات الـ Errors والـ Enums بتاعتك

namespace WearCast.Api.Features.Sellers.SellerApplications.ApplyForSelling
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
                // أضفنا Trim() عشان نطير أي مسافات من اليمين أو الشمال جاية من الريكويست
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

                // خلينا الـ Tax ID ليه شرط صريح 
                if (existingApp.TaxIdNumber.Equals(request.SellerTaxIdNumber?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure(SellerErrors.TaxIdInUse);

                // Fallback عشان لو حصلت أي مشكلة مسافات غريبة الـ C# مقدرتش تقفشها
                return Result.Failure(new Error("Application.DataConflict", "One of the provided unique fields is already in use.", StatusCodes.Status400BadRequest));
            }


            var existingRejectedApp = await _context.SellerApplications
                .Where(x => x.ManagerEmail == request.SellerManagerEmail && x.Status == Status.Rejected)
                .FirstOrDefaultAsync(cancellationToken);

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