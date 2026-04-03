using WearCast.Api.Entities.FixedProduct;
using WearCast.Api.Features.FixedProductColor.AddFixedProductImage.DTOs;
using WearCast.Api.Features.FixedProductColor.Errors;

namespace WearCast.Api.Features.FixedProductColor.AddFixedProductImage;

public class AddFixedProductImageHandler : IRequestHandler<AddFixedProductImageCommandDto, Result>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepository;
    private readonly IRepository<FixedProductImage> _imageRepository;
    private readonly ImageService _imageService;

    public AddFixedProductImageHandler(
        IRepository<Entities.FixedProduct.FixedProductColor> colorRepository,
        IRepository<FixedProductImage> imageRepository,
        ImageService imageService)
    {
        _colorRepository = colorRepository;
        _imageRepository = imageRepository;
        _imageService = imageService;
    }

    public async Task<Result> Handle(AddFixedProductImageCommandDto request, CancellationToken cancellationToken)
    {
        var color = await _colorRepository.Get()
        .Include(c => c.Product)
        .FirstOrDefaultAsync(c => c.Id == request.ProductColorId, cancellationToken);

        if (color is null)
        {
            return Result.Failure(FixedProductColorErrors.ColorNotFound);
        }

        if (!request.isAdminRequest && color.Product.SellerId != request.sellerId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        var imageValidation = _imageService.Validate(request.Image!);
        if (!imageValidation.IsValid)
        {
            return Result.Failure(new Error("Product.InvalidImage", imageValidation.ErrorMessage, 400));
        }

        string? url = await _imageService.UploadAsync(request.Image);
        if (string.IsNullOrEmpty(url))
        {
            return Result.Failure(FixedProductColorErrors.UploadFailed);
        }

        var newImage = new FixedProductImage
        {
            ProductColorId = request.ProductColorId,
            ImageUrl = url
        };

        await _imageRepository.CreateAsync(newImage);

        return Result.Success();
    }
}