namespace WearCast.Api.Features.ShippingCompanies.UpdateShippingCompanyImage
{
    public class UpdateShippingCompanyImageHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService
        ) : IRequestHandler<UpdateShippingCompanyImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;
        public async Task<Result> Handle(UpdateShippingCompanyImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetShippingCompanyId;

            if (user.IsSuperAdmin())
            {
                if (!request.ProvidedShippingCompanyId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target ProviderId to delete.", StatusCodes.Status400BadRequest));
                }

                targetShippingCompanyId = request.ProvidedShippingCompanyId.Value;
            }
            else
            {
                targetShippingCompanyId = user.GetShippingCompanyId()!.Value;
            }

            var shippingCompany = await _context.ShippingCompanies
                .FirstOrDefaultAsync(x => x.Id == targetShippingCompanyId, cancellationToken);

            if (shippingCompany == null)
            {
                return Result.Failure(ShippingCompanyErrors.CompanyNotFound);
            }

            var oldLogoUrl = shippingCompany.LogoUrl;

            var newLogoUrl = await _imageService.UploadAsync(request.NewLogo);
            if (newLogoUrl == null)
            {
                return Result.Failure(ImageErrors.UploadFailed);
            }

            shippingCompany.LogoUrl = newLogoUrl;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                await _imageService.DeleteAsync(newLogoUrl);
                return Result.Failure(new Error("Database.SaveFailed", "Failed to save the new Logo to the database.", StatusCodes.Status500InternalServerError));
            }

            await _imageService.DeleteAsync(oldLogoUrl);

            return Result.Success();
        }
    }
}
