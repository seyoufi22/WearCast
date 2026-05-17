namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.AddFactoryProductColor
{
    public record AddFactoryProductColorRequest(
        string Name,
        string HexCode,
        IFormFile Image,
        int ProductId
        ) : IRequest<Result<AddFactoryProductColorResponse>>;
}
