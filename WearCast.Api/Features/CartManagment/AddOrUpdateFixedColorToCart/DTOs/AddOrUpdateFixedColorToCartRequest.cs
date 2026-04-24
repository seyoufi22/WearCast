namespace WearCast.Api.Features.CartManagment.AddOrUpdateFixedColorToCart.DTOs;

public record SizeQuantityItem(Size Size, int Quantity);

public record AddOrUpdateFixedColorToCartRequest(int ColorId, List<SizeQuantityItem> Sizes);