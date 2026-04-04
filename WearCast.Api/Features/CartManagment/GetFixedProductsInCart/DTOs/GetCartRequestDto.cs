namespace WearCast.Api.Features.CartManagment.GetFixedProductsInCart.DTOs;

public record GetCartRequestDto(int CustomerId) : IRequest<List<GetCartItemResponseDto>>;
