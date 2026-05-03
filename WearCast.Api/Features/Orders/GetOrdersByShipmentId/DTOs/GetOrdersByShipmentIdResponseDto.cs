using WearCast.Api.Common.Enums;
using WearCast.Api.Common.ValueObjects;

namespace WearCast.Api.Features.Orders.GetOrdersByShipmentId.DTOs;

public class GetOrdersByShipmentIdResponseDto
{
    public int ShipmentId { get; set; }
    public ShipmentStatus ShipmentStatus { get; set; }
    public Address DeliveryAddress { get; set; } = new();
    public List<ShipmentOrderDto> Orders { get; set; } = new();
}

public class ShipmentOrderDto
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Commission { get; set; }
    public decimal Payout { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientPhoneNumber { get; set; } = string.Empty;
    public Address ShippingAddress { get; set; } = new();

    /// <summary>
    /// "Fixed" or "Designed" — indicates whether this order contains fixed or designed items.
    /// </summary>
    public string OrderType { get; set; } = string.Empty;
}
