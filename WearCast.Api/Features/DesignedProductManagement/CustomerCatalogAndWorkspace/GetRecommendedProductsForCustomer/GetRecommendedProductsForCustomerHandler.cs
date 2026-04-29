using WearCast.Api.Common.ExternalServices;
using WearCast.Api.Common.Helper;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetRecommendedProductsForCustomer
{
    public class GetRecommendedProductsForCustomerHandler(
        ApplicationDbContext context,
        IRecommendationServiceClient recommendationClient,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetRecommendedProductsForCustomerRequest, Result<List<GetRecommendedProductsForCustomerResponse>>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IRecommendationServiceClient _recommendationClient = recommendationClient;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<List<GetRecommendedProductsForCustomerResponse>>> Handle(GetRecommendedProductsForCustomerRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.IsCustomer())
            {
                return Result.Failure<List<GetRecommendedProductsForCustomerResponse>>(new Error("User.NotFound", "User not found or not a customer", StatusCodes.Status404NotFound));
            }

            var userId = user.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Result.Failure<List<GetRecommendedProductsForCustomerResponse>>(new Error("User.IdNotFound", "User ID not found", StatusCodes.Status404NotFound));
            }

            // 1. Get product IDs from ML service
            Console.WriteLine($"[Recommendation] [START] Fetching AI recommendations for User: {userId}");
            var recommendedProductIds = await _recommendationClient.GetRecommendationsAsync(userId, request.TopK);
            Console.WriteLine($"[Recommendation] [TRACE] ML Service returned {recommendedProductIds.Count} raw IDs.");

            if (recommendedProductIds.Count == 0)
            {
                Console.WriteLine($"[Recommendation] [FALLBACK] Empty AI results. Using Newest Products fallback.");
                // Fallback: Return newest designed products if no recommendations
                var newestProducts = await _context.DesignedProducts
                    .AsNoTracking()
                    .OrderByDescending(p => p.CreatedOn)
                    .Take(request.TopK)
                    .Select(p => new GetRecommendedProductsForCustomerResponse
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        CategoryName = p.Category.Name,
                        MainImageUrl = p.DefaultColor != null 
                            ? p.DefaultColor.MainImageUrl 
                            : p.Colors.Select(c => c.MainImageUrl).FirstOrDefault()
                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(newestProducts);
            }

            // 2. Parse Prefixes and separate IDs
            var forFixed = new List<int>();
            var forDesigned = new List<int>();
            
            foreach (var fullId in recommendedProductIds)
            {
                if (fullId.StartsWith("FIX_"))
                {
                    if (int.TryParse(fullId.Replace("FIX_", ""), out int id)) forFixed.Add(id);
                }
                else if (fullId.StartsWith("DES_"))
                {
                    if (int.TryParse(fullId.Replace("DES_", ""), out int id)) forDesigned.Add(id);
                }
                else if (int.TryParse(fullId, out int id))
                {
                    forDesigned.Add(id);
                }
            }
            Console.WriteLine($"[Recommendation] [PARSE] IDs categorized: {forFixed.Count} Fixed, {forDesigned.Count} Designed.");

            // 3. Fetch from both tables
            Console.WriteLine($"[Recommendation] [DB] Fetching product metadata for {forFixed.Count + forDesigned.Count} items.");
            var designedProducts = await _context.DesignedProducts
                .AsNoTracking()
                .Where(p => forDesigned.Contains(p.Id))
                .Include(p => p.Category)
                .Select(p => new { p.Id, p.Name, p.Price, CategoryName = p.Category.Name, Image = p.DefaultColor.MainImageUrl ?? p.Colors.Select(c => c.MainImageUrl).FirstOrDefault(), IsDesigned = true })
                .ToListAsync(cancellationToken);

            var fixedProducts = await _context.FixedProducts
                .AsNoTracking()
                .Where(p => forFixed.Contains(p.Id))
                .Include(p => p.Category)
                .Select(p => new { p.Id, p.Name, p.Price, CategoryName = p.Category.Name, Image = p.Colors.Select(c => c.ImageUrl).FirstOrDefault(), IsDesigned = false })
                .ToListAsync(cancellationToken);

            // 4. Merge results maintaining AI rank order
            var finalResponses = new List<GetRecommendedProductsForCustomerResponse>();
            foreach (var fullId in recommendedProductIds)
            {
                if (fullId.StartsWith("FIX_"))
                {
                    int id = int.Parse(fullId.Replace("FIX_", ""));
                    var p = fixedProducts.FirstOrDefault(x => x.Id == id);
                    if (p != null) finalResponses.Add(new GetRecommendedProductsForCustomerResponse { Id = p.Id, Name = p.Name, Price = p.Price, CategoryName = p.CategoryName, MainImageUrl = p.Image });
                }
                else
                {
                    string cleanId = fullId.Replace("DES_", "");
                    if (int.TryParse(cleanId, out int id))
                    {
                        var p = designedProducts.FirstOrDefault(x => x.Id == id);
                        if (p != null) finalResponses.Add(new GetRecommendedProductsForCustomerResponse { Id = p.Id, Name = p.Name, Price = p.Price, CategoryName = p.CategoryName, MainImageUrl = p.Image });
                    }
                }
            }

            Console.WriteLine($"[Recommendation] [COMPLETE] Successfully hydrated {finalResponses.Count} items. Returning result.");
            return Result.Success(finalResponses);
        }
    }
}
