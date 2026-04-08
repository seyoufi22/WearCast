namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsStatusForSeller.DTOs;

public record GetAllFixedProductsStatusForSellerRequestDto(int SellerId) : IRequest<Result<GetAllFixedProductsStatusForSellerResponseDto>>;
