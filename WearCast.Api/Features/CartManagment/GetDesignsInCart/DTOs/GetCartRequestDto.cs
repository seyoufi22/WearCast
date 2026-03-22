namespace WearCast.Api.Features.CartManagment.GetDesignsInCart.DTOs;
public record GetCartRequestDto(int CustomerId) : IRequest<List<GetCartItemResponseDto>>;