namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColor
{
    public record UpdateFactoryProductColorRequest(
         int ColorId,
         string Name,
         string HexCode,
         int ProductId
     ) : IRequest<Result>;
}
