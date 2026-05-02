namespace WearCast.Api.Features.Sellers.UpdateSeller
{
    public class UpdateSellerHandler(
         ApplicationDbContext context,
         IHttpContextAccessor httpContextAccessor,
         IMapper mapper
        ) : IRequestHandler<UpdateSellerRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;
        public async Task<Result> Handle(UpdateSellerRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetSellerId;

            if (user.IsSuperAdmin() || user.IsVendorAdmin())
            {
                if (!request.ProvidedSellerId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target ProviderId to delete.", StatusCodes.Status400BadRequest));
                }

                targetSellerId = request.ProvidedSellerId.Value;
            }
            else
            {
                targetSellerId = user.GetSellerId()!.Value;
            }

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.Id == targetSellerId, cancellationToken);

            if (seller == null)
            {
                return Result.Failure(SellerErrors.SellerNotFound);
            }
            var conflict = await _context.Sellers
                .AsNoTracking()
                .Where(x => x.Id != targetSellerId &&
                           (x.CommercialRegisterNumber == request.CommercialRegisterNumber ||
                            x.TaxIdNumber == request.TaxIdNumber))
                .Select(x => new { x.CommercialRegisterNumber, x.TaxIdNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (conflict != null)
            {
                if (conflict.CommercialRegisterNumber == request.CommercialRegisterNumber)
                {
                    return Result.Failure(SellerErrors.CommercialRegisterInUse);
                }

                if (conflict.TaxIdNumber == request.TaxIdNumber)
                {
                    return Result.Failure(SellerErrors.TaxIdInUse);
                }
            }
            _mapper.Map(request, seller);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
    }
}
