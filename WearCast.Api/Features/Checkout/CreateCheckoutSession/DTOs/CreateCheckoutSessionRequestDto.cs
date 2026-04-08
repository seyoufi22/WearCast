using WearCast.Api.Abstractions;

namespace WearCast.Api.Features.Checkout.CreateCheckoutSession.DTOs;

public record CreateCheckoutSessionRequestDto : IRequest<Result<CreateCheckoutSessionResponseDto>>
{
    [System.Text.Json.Serialization.JsonIgnore]
    public int CustomerId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public string CustomerEmail { get; set; } = string.Empty;

    public ShippingInfoDto ShippingInfo { get; set; } = new();
}

