namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColor
{
    public class UpdateFactoryProductColorHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<UpdateFactoryProductColorRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(UpdateFactoryProductColorRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;


            var queryResult = await _context.DesignedProductColors
                .Where(x => x.Id == request.ColorId && x.DesignedProduct.Id == request.ProductId)
                .Select(x => new
                {
                    Color = x,
                    FactoryId = x.DesignedProduct.FactoryId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (queryResult == null)
            {
                return Result.Failure(FactoryProductColorErrors.ColorNotFound);
            }

            var color = queryResult.Color;
            var productFactoryId = queryResult.FactoryId;

            if (user.IsSuperAdmin())
            {
                // No Action 
            }
            else if (user.IsFactoryManager())
            {
                var factoryIdFromToken = user.GetFactoryId();

                if (factoryIdFromToken == null)
                {
                    return Result.Failure(AuthErrors.NoAssociatedFactory);
                }

                if (productFactoryId != factoryIdFromToken.Value)
                {
                    return Result.Failure(AuthErrors.Forbidden);
                }
            }
            else
            {
                return Result.Failure(AuthErrors.Forbidden);
            }

            color.Name = request.Name;
            color.HexCode = request.HexCode;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}