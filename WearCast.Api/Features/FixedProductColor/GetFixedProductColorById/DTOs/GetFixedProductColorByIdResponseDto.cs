namespace WearCast.Api.Features.FixedProductColor.GetFixedProductColorById.DTOs;

public record GetFixedProductColorByIdResponseDto(
    int Id,
    string ColorName,
    string ColorCode,
    string ImageUrl,
    List<SizeDetailsDto> Sizes,
    List<ImageDetailsDto> AdditionalImages 
);

public record SizeDetailsDto(string Size, int Quantity);

public record ImageDetailsDto(int Id, string ImageUrl);
