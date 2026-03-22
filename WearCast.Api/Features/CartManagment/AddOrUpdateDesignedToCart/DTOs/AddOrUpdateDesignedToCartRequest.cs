namespace WearCast.Api.Features.CartManagment.AddOrUpdateDesignedToCart.DTOs;

public record AddOrUpdateDesignedToCartRequest(int DesignId, Size Size, int Quantity) : IRequest<AddOrUpdateDesignedToCartRequest>;

