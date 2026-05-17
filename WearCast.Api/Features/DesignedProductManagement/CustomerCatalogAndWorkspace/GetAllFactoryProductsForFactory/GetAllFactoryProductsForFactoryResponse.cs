using System.Text.Json.Serialization;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetAllFactoryProductsForFactory
{
    public record GetAllFactoryProductsForFactoryResponse
    {
        public int Id { get; init; }
        public int CategoryId { get; init; }
        public string CategoryName { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }

        [JsonIgnore]
        public TargetAudience TargetAudienceEnum { get; init; }
        public List<string> TargetAudiences => TargetAudienceEnum.ToString()
                                                .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                                                .ToList();

        public int? DefaultColorId { get; init; }
        public string? MainImageUrl { get; init; }

        public decimal AverageRating { get; init; }
        public int ReviewCount { get; init; }
    }
}
