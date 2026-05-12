using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsForSeller.DTOs;

public record GetAllFixedProductsForSellerRequestDto : IRequest<Result<PagingViewModel<GetAllFixedProductsForSellerResponseDto>>>{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 100;
    public int? CategoryId { get; set; } = null;
    public StockStatus? StockStatus { get; set; } = null;
    public string? SearchTerm { get; set; } = null;
    public bool? IsRejected { get; set; } = null;
    internal int SellerId { get; set; } = 0;

}

