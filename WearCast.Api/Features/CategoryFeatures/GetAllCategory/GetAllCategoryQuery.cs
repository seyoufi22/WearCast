namespace WearCast.Api.Features.CategoryFeatures.GetAllCategory;

public record GetCategoriesQuery : IRequest<List<CategoryResponse>>;
public record CategoryResponse(int Id, string Name);