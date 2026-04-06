namespace WearCast.Api.Features.Checkout.CreateCheckoutSession.DTOs;

public record CreateCheckoutSessionResponseDto
{
    public string CheckoutUrl { get; init; } = string.Empty;
    public List<int> OrderIds { get; init; } = new();
}
