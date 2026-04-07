namespace WearCast.Api.Features.Checkout.CreateCheckoutSession.DTOs;

public class ShippingInfoDto
{
    public string RecipientName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? AdditionalPhoneNumber { get; set; }
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string BuildingNumber { get; set; } = string.Empty;
}
