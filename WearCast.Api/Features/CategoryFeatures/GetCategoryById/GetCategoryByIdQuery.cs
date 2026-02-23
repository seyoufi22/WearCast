namespace WearCast.Api.Features.CategoryFeatures.GetCategoryById;

public record GetCategoryByIdQuery(int Id) : IRequest<CategoryResponse>;

public record CategoryResponse(string Name, string ImageUrl);