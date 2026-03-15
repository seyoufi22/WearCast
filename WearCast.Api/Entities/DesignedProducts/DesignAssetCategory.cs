

namespace WearCast.Api.Entities.DesignedProducts
{
    public class DesignAssetCategory : BaseModel, ISoftDeletable
    {
        public string Name { get; set; } = string.Empty;
        // public int SortOrder { get; set; } // عشان نرتب الأقسام في الفرانتد زي ما المصنع عايز

        public ICollection<DesignAsset> Assets { get; set; } = [];
    }
}
