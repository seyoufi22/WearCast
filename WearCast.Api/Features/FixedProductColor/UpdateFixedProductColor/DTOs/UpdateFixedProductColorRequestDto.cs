namespace WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor.DTOs;

public class UpdateFixedProductColorRequestDto 
{
    public int ColorId { get; set; }
    public string ColorName { get; set; }
    public string ColorCode { get; set; }
    public IFormFile? Image { get; set; }
}

