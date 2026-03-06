namespace WearCast.Api.Features.Category.GetCategoryById.DTOs;

public record GetCategoryByIdRequestDto(int Id) : IRequest<CategoryResponse>;