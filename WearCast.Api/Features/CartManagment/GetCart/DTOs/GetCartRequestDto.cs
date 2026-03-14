namespace WearCast.Api.Features.CartManagment.GetCart.DTOs;

public record GetCartRequestDto(int CustomerId) : IRequest<List<GetCartItemResponseDto>>;
