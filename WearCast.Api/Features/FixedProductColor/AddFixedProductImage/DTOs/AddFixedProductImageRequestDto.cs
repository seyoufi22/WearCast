namespace WearCast.Api.Features.FixedProductColor.AddFixedProductImage.DTOs;

public record AddFixedProductImageRequestDto(int ProductColorId, IFormFile Image);
