namespace WearCast.Api.Features.ShippingCompanies.UpdateShippingCompany
{
    public class UpdateShippingCompanyHandler(
         ApplicationDbContext context,
         IHttpContextAccessor httpContextAccessor,
         IMapper mapper
        ) : IRequestHandler<UpdateShippingCompanyRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;
        public async Task<Result> Handle(UpdateShippingCompanyRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetCompanyId;

            if (user.IsSuperAdmin())
            {
                if (!request.ProvidedCompanyId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target ProviderId to delete.", StatusCodes.Status400BadRequest));
                }

                targetCompanyId = request.ProvidedCompanyId.Value;
            }
            else
            {
                targetCompanyId = user.GetShippingCompanyId()!.Value;
            }

            var company = await _context.ShippingCompanies
                .FirstOrDefaultAsync(x => x.Id == targetCompanyId, cancellationToken);

            if (company == null)
            {
                return Result.Failure(ShippingCompanyErrors.CompanyNotFound);
            }

            _mapper.Map(request, company);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
    }
}
