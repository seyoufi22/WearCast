using MediatR;
using WearCast.Api.Common.Repository; // مسار الـ IRepository بتاعك
using WearCast.Api.Entities.FixedProduct; // مسار الـ FixedProductImage
using WearCast.Api.Features.FixedProductColor.DeleteFixedProductImage.DTOs;

namespace WearCast.Api.Features.FixedProductColor.DeleteFixedProductImage;

public class DeleteFixedProductImageHandler : IRequestHandler<DeleteFixedProductImageRequestDto, bool>
{
    private readonly IRepository<FixedProductImage> _imageRepository;
    private readonly ImageService _imageService;
    public DeleteFixedProductImageHandler(IRepository<FixedProductImage> imageRepository, ImageService imageService)
    {
        _imageRepository = imageRepository;
        _imageService = imageService;
    }

    public async Task<bool> Handle(DeleteFixedProductImageRequestDto request, CancellationToken cancellationToken)
    {
        var image = await _imageRepository.GetAsync(
            img => img.Id == request.ImageId
        );

        if (image == null)
        {
            return false;
        }
        await _imageService.DeleteAsync(image.ImageUrl);
        await _imageRepository.HardDeleteAsync(image);

        return true;
    }
}