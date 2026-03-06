using WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor.DTOs;

namespace WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor;

public class UpdateFixedProductColorHandler : IRequestHandler<UpdateFixedProductColorRequestDto, bool>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;
    private readonly ImageService _imageService; 

    public UpdateFixedProductColorHandler(IRepository<Entities.FixedProduct.FixedProductColor> colorRepo, ImageService imageService)
    {
        _colorRepo = colorRepo;
        _imageService = imageService;
    }

    public async Task<bool> Handle(UpdateFixedProductColorRequestDto request, CancellationToken cancellationToken)
    {
        var color = await _colorRepo.GetAsync(c => c.Id == request.ColorId);

        if (color == null)
            return false;

        color.ColorName = request.ColorName.Trim();
        color.ColorCode = request.ColorCode.Trim().ToUpper();

        if (request.Image != null)
        {

            await _imageService.DeleteAsync(color.ImageUrl);

            color.ImageUrl = await _imageService.UploadAsync(request.Image);
        }

        await _colorRepo.UpdateAsync(color);

        return true;
    }
}