namespace WearCast.Api.Features.CartManagment.GetMyCart.DTOs;

public record GetCartRequestDto(int CustomerId) : IRequest<CartSummaryResponseDto>;