using MediatR;
using WearCast.Api.Common.Repository;
using WearCast.Api.Entities.FixedProduct;
using WearCast.Api.Features.FixedProductColor.AddFixedProductImage.DTOs;
using WearCast.Api.Common.Services;

namespace WearCast.Api.Features.FixedProductColor.AddFixedProductImage;

public class AddFixedProductImageHandler : IRequestHandler<AddFixedProductImageRequestDto, bool>
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

    public async Task<bool> Handle(AddFixedProductImageRequestDto request, CancellationToken cancellationToken)
    {
        var color = await _colorRepository.GetAsync(c => c.Id == request.ProductColorId, useNoTracking: true);
        if (color == null) return false;

        var uploadedUrl = await _imageService.UploadAsync(request.File);

        var newImage = new FixedProductImage
        {
            ProductColorId = request.ProductColorId,
            ImageUrl = uploadedUrl
        };

        await _imageRepository.CreateAsync(newImage);

        return true;
    }
}