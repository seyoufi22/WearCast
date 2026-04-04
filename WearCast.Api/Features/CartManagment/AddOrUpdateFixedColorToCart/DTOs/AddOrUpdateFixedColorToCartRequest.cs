namespace WearCast.Api.Features.CartManagment.AddOrUpdateFixedColorToCart.DTOs;

public record AddOrUpdateFixedColorToCartRequest(int ColorId,Size Size,int Quantity) : IRequest<AddOrUpdateFixedColorToCartRequest>;
