namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.AddCustomerDesign
{
    public record AddCustomerDesignRequest(
        string? Name,
        string ViewDesignsJson,
        IFormFile? FrontImage,
        IFormFile? BackImage,
        IFormFile? RightImage,
        IFormFile? LeftImage,
        int AssetCount,
        int ProductId,
        int ProductColorId
        ) : IRequest<Result<CustomerDesignResponse>>;
}
