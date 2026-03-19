namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.GetCustomerDesign
{
    public record GetCustomerDesignResponse(
         int Id,
         int ProductId,
         int ProductColorId,
         string ViewDesignsJson
     );
}
