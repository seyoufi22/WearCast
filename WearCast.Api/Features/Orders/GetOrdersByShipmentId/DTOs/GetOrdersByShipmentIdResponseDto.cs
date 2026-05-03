using WearCast.Api.Common.Enums;
using WearCast.Api.Common.ValueObjects;

namespace WearCast.Api.Features.Orders.GetOrdersByShipmentId.DTOs;

public class GetOrdersByShipmentIdResponseDto
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public Address DeliveryAddress { get; set; } = new();
    public decimal Price { get; set; }
    public ShipmentStatus ShipmentStatus { get; set; }
    public DateTime OrderTime { get; set; }
    public DateTime? ReadyForPickupAt { get; set; }
    public DateTime? TripStartedAt { get; set; }
    public DateTime? OutForDeliveryAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string DeliveryCode { get; set; } = string.Empty;
    public int? DriverId { get; set; }
    public string? DriverName { get; set; }
    public string? DriverPhoneNumber { get; set; }
    public string? DriverNationalId { get; set; }

    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhoneNumber { get; set; } = string.Empty;

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
