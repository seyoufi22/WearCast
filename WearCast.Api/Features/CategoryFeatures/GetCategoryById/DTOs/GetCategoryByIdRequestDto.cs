namespace WearCast.Api.Features.CategoryFeatures.GetCategoryById.DTOs;

public record GetCategoryByIdRequestDto(int Id) : IRequest<CategoryResponse>;