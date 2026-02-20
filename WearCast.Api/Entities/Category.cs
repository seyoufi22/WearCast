namespace WearCast.Api.Entities
{
    public class Category : BaseModel
    {
        public string Name { get; set; }    
        public string ImageUrl { get; set; }

        public ICollection<FixedProduct.FixedProduct> Products { get; set; }
    }
}
