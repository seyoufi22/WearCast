namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.AddCustomerDesign
{
    public record AddCustomerDesignRequest(
        string ViewDesignsJson,
        IFormFile? FrontImage,
        IFormFile? BackImage,
        IFormFile? RightImage,
        IFormFile? LeftImage,
        int ProductId,
        int ProductColorId
        ) : IRequest<Result<CustomerDesignResponse>>;
}
