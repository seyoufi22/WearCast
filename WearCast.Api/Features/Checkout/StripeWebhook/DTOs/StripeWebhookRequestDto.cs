using WearCast.Api.Abstractions;

namespace WearCast.Api.Features.Checkout.StripeWebhook.DTOs;

public record StripeWebhookRequestDto(string StripeSessionId) : IRequest<Result<bool>>;
