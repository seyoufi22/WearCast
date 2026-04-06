using WearCast.Api.Abstractions;

namespace WearCast.Api.Features.Checkout.GetShippingInfo.DTOs;

public record GetShippingInfoRequestDto(int CustomerId)
    : IRequest<Result<GetShippingInfoResponseDto>>;
