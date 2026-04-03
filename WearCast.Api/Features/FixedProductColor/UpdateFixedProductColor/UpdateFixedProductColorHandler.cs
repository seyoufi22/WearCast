using WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor.DTOs;
using WearCast.Api.Features.FixedProductColor.Errors;
namespace WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor;

public class UpdateFixedProductColorHandler : IRequestHandler<UpdateFixedProductColorCommandDto, Result>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;
    private readonly ImageService _imageService; 

    public UpdateFixedProductColorHandler(IRepository<Entities.FixedProduct.FixedProductColor> colorRepo, ImageService imageService)
    {
        _colorRepo = colorRepo;
        _imageService = imageService;
    }

    public async Task<Result> Handle(UpdateFixedProductColorCommandDto command, CancellationToken cancellationToken)
    {
        var color = await _colorRepo.Get().Include(c=> c.Product).FirstOrDefaultAsync(c => c.Id == command.request.ColorId);
        

        if (color is null)
            return Result.Failure(FixedProductColorErrors.ColorNotFound);
        
        if(!command.isAdminRequest && color.Product.SellerId != command.sellerId)
            return Result.Failure(FixedProductColorErrors.ColorNotFound);

        bool isExistingColorCode = await _colorRepo.Get()
            .AnyAsync(c => c.ColorCode == command.request.ColorCode.Trim().ToUpper() && c.Id != command.request.ColorId);
        
        if(isExistingColorCode)
            return Result.Failure(FixedProductColorErrors.DuplicateHexCode);

        color.ColorName = command.request.ColorName.Trim();
        color.ColorCode = command.request.ColorCode.Trim().ToUpper();

        if (command.request.Image != null)
        {
            var mainImageValidation = _imageService.Validate(command.request.Image);
            if (!mainImageValidation.IsValid)
            {
                return Result.Failure(new Error("Product.MainImageInvalid", mainImageValidation.ErrorMessage, 400));
            }

            string? mainUrl = await _imageService.UploadAsync(command.request.Image);
            if (string.IsNullOrEmpty(mainUrl))
            {
                return Result.Failure(FixedProductColorErrors.UploadFailed);
            }

            await _imageService.DeleteAsync(color.ImageUrl);

            color.ImageUrl = mainUrl;
        }

        await _colorRepo.UpdateAsync(color);

        return Result.Success();
    }
}