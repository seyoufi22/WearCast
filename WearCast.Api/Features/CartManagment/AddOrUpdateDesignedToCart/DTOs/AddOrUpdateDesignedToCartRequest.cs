namespace WearCast.Api.Features.CartManagment.AddOrUpdateDesignedToCart.DTOs;

public record SizeQuantityItem(Size Size, int Quantity);
public record AddOrUpdateDesignedToCartRequest(int DesignId, List<SizeQuantityItem> Sizes) : IRequest<AddOrUpdateDesignedToCartRequest>;

