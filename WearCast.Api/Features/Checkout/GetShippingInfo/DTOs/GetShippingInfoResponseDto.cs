namespace WearCast.Api.Features.Checkout.GetShippingInfo.DTOs;

public class GetShippingInfoResponseDto
{
    public string RecipientName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? AdditionalPhoneNumber { get; set; }
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string BuildingNumber { get; set; } = string.Empty;
}
