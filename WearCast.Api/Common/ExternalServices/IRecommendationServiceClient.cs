namespace WearCast.Api.Common.ExternalServices
{
    public interface IRecommendationServiceClient
    {
        Task<List<string>> GetRecommendationsAsync(string userId, int topK = 10);
    }
}
