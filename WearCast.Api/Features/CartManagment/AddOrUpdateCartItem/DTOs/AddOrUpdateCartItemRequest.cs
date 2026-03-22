namespace WearCast.Api.Features.CartManagement.AddOrUpdateCartItem.DTOs;

public record AddOrUpdateCartItemRequest(int ColorId,Size Size,int Quantity) : IRequest<AddOrUpdateCartItemRequest>;
