namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColor
{
    public record UpdateFactoryProductColorRequest(
         string ProductSlug,
         string CurrentColorSlug,
         string Name,
         string HexCode
     ) : IRequest<Result>;
}
