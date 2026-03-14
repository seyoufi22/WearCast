namespace WearCast.Api.Entities.DesignedProducts
{
    public class CustomerUploadedImage : BaseModel
    {
        public string ImageUrl { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
