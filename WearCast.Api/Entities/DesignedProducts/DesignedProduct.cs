namespace WearCast.Api.Entities.DesignedProducts
{
    public class DesignedProduct : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TargetAudience TargetAudience { get; set; }
        public decimal Price { get; set; }
        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int FactoryId { get; set; }
        public Factory? Factory { get; set; }

        public ICollection<DesignedProductColor> Colors { get; set; } = [];
        public ICollection<DesignedProductSizeDetails> SizeDetails { get; set; } = [];
    }
}
