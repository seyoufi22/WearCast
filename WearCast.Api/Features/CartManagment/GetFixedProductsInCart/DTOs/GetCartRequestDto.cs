using MediatR;

namespace WearCast.Api.Features.CartManagment.GetFixedProductsInCart.DTOs;

// The return type has been changed to the new wrapper DTO
public record GetCartRequestDto(int CustomerId) : IRequest<FixedCartSummaryDto>;