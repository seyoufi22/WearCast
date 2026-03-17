namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.AddFactoryProductColor
{
    public class AddFactoryProductColorHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<AddFactoryProductColorRequest, Result<AddFactoryProductColorResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<AddFactoryProductColorResponse>> Handle(AddFactoryProductColorRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;


            var factoryId = await _context.DesignedProducts
                 .Where(x => x.Id == request.ProductId)
                 .Select(x => (int?)x.FactoryId)
                 .FirstOrDefaultAsync(cancellationToken);

            if (factoryId == null)
            {
                return Result.Failure<AddFactoryProductColorResponse>(DesignedProductErrors.ProductNotFound);
            }

            if (user.IsSuperAdmin())
            {
                //No Action
            }
            else if (user.IsFactoryManager())
            {
                var factoryIdFromToken = user.GetFactoryId();

                if (factoryIdFromToken == null)
                {
                    return Result.Failure<AddFactoryProductColorResponse>(AuthErrors.NoAssociatedFactory);
                }

                if (factoryId != factoryIdFromToken.Value)
                {
                    return Result.Failure<AddFactoryProductColorResponse>(AuthErrors.Forbidden);
                }
            }
            else
            {
                return Result.Failure<AddFactoryProductColorResponse>(AuthErrors.Forbidden);
            }

            var colorExists = await _context.DesignedProductColors
                .AnyAsync(c => c.DesignedProductId == request.ProductId && c.HexCode == request.HexCode, cancellationToken);

            if (colorExists)
            {
                return Result.Failure<AddFactoryProductColorResponse>(FactoryProductColorErrors.ColorAlreadyExists);
            }

            var newProductColor = new DesignedProductColor
            {
                Name = request.Name,
                HexCode = request.HexCode,
                DesignedProductId = request.ProductId
            };

            _context.DesignedProductColors.Add(newProductColor);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new AddFactoryProductColorResponse(newProductColor.Id));
        }
    }
}
