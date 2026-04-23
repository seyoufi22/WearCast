using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsForAdmin.DTOs;

public record GetAllFixedProductsForAdminRequestDto : IRequest<Result<PagingViewModel<GetAllFixedProductsForAdminResponseDto>>>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 30; 
    public string? SearchTerm { get; set; } = null;

}