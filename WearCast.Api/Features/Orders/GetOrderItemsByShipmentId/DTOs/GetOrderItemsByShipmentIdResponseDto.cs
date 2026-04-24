using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.Orders.GetOrderItemsByShipmentId.DTOs;

public class GetOrderItemsByShipmentIdResponseDto
{
    public PagingViewModel<FixedProductOrderItemDto> FixedItems { get; set; } = new();
    public PagingViewModel<DesignedProductOrderItemDto> DesignedItems { get; set; } = new();
}
