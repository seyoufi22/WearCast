using System.Text.Json.Serialization;

namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductColor.DTOs;

public record CreateFixedProductColorRequestDto(
    int ProductId,
    string ColorName,
    string ColorCode,
    IFormFile Image,
    List<IFormFile>? AdditionalImages,
    [ModelBinder(BinderType = typeof(JsonModelBinder))] List<CreateSizeDto> Sizes
) : IRequest<Result>;

public class CreateSizeDto
{
    [JsonPropertyName("size")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Size Size { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
