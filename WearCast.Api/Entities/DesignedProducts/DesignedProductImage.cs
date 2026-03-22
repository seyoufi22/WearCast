namespace WearCast.Api.Entities.DesignedProducts
{
    public class DesignedProductImage : BaseModel, ISoftDeletable
    {
        public string ImageUrl { get; set; } = string.Empty;

        public ViewSide ViewSide { get; set; }

        public int DesignedProductColorId { get; set; }
        public DesignedProductColor DesignedProductColor { get; set; } = default!;
    }
}
