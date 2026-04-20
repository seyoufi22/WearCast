namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.GetAllCustomerDesigns
{
    public record GetAllCustomerDesignsResponse(
        int Id,
        int DesignedProductId,
        string DesignName,
        string ProductName,
        decimal TotalPrice,
        string? Image,
        DateTime CreatedOn
        );
}
