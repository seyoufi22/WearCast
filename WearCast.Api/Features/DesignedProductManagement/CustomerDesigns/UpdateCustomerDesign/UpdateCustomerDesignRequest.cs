namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.UpdateCustomerDesign
{
    public record UpdateCustomerDesignRequest(
        int Id,
        string ViewDesignsJson,
        IFormFile? FrontImage,
        IFormFile? BackImage,
        IFormFile? RightImage,
        IFormFile? LeftImage,
        int AssetCount
        ) : IRequest<Result<CustomerDesignResponse>>;
}
