namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.AddFactoryProductColor
{
    public record AddFactoryProductColorRequest(
        string ProductSlug,
        string Name,
        string HexCode
        ) : IRequest<Result<AddFactoryProductColorResponse>>;
}
