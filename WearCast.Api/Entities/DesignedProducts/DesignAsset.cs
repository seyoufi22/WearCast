namespace WearCast.Api.Entities.DesignedProducts
{
    public class DesignAsset : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public int WidthPx { get; set; }
        public int HeightPx { get; set; }

        // public int SortOrder { get; set; } // ترتيب الاستيكر جوه القسم بتاعه

        public int DesignAssetCategoryId { get; set; }
        public DesignAssetCategory DesignAssetCategory { get; set; } = default!;
    }
}
