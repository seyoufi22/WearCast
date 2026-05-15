using WearCast.Api.Entities.FixedProduct; 
using WearCast.Api.Features.FixedProductColor.DeleteFixedProductImage.DTOs;
using WearCast.Api.Features.FixedProductColor.Errors;

namespace WearCast.Api.Features.FixedProductColor.DeleteFixedProductImage;

public class DeleteFixedProductImageHandler : IRequestHandler<DeleteFixedProductImageRequestDto, Result>
{
    private readonly IRepository<FixedProductImage> _imageRepository;
    private readonly ImageService _imageService;
    public DeleteFixedProductImageHandler(IRepository<FixedProductImage> imageRepository, ImageService imageService)
    {
        _imageRepository = imageRepository;
        _imageService = imageService;
    }

    public async Task<Result> Handle(DeleteFixedProductImageRequestDto request, CancellationToken cancellationToken)
    {
        var image = await _imageRepository.Get().Include(c => c.ProductColor).ThenInclude(c=>c.Product)
            .FirstOrDefaultAsync(img => img.Id == request.ImageId);

        if (image is null) return Result.Failure(FixedProductColorErrors.ImageNotFound);

        if(!request.isAdminRequest && image.ProductColor.Product.SellerId != request.sellerId) 
            return Result.Failure(AuthErrors.Forbidden);

        await _imageService.DeleteAsync(image.ImageUrl);
        await _imageRepository.HardDeleteAsync(image);

        return Result.Success();
    }
}