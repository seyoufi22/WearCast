namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.AddFactoryProductColor
{
    public record AddFactoryProductColorRequest(
        int ProductId,
        string Name,
        string HexCode
        ) : IRequest<Result<AddFactoryProductColorResponse>>;
}
