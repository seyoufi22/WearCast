using WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.CreateShippingCompanyManager;

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
            var shippingCompanyId = await context.ShippingCompanies
                 .AsNoTracking()
                 .Where(x => !x.IsDeleted)
                 .Select(s => (int?)s.Id)
                 .FirstOrDefaultAsync(cancellationToken);

            if (shippingCompanyId == null)
                return Result.Failure<CreateShippingCompanyManagerResponse>(new Error("ShippingCompany.NotFound", "Thier is no shipping company yet.", StatusCodes.Status404NotFound));

            var company = await _context.ShippingCompanies
                .FirstOrDefaultAsync(x => x.Id == shippingCompanyId.Value, cancellationToken);

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
