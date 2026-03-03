namespace WearCast.Api.Entities.DesignedProducts
{
    public class DesignedProduct : BaseModel
    {
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TargetAudience TargetAudience { get; set; }
        public int Price { get; set; }
        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }

        public ICollection<DesignedProductColor> Colors { get; set; } = [];
    }
}
