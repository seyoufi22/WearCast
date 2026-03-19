namespace WearCast.Api.Features.FixedProductColor.AddFixedProductSize.DTOs;

public record AddFixedProductSizeRequestDto(int ColorId, Size Size, int Quantity);
