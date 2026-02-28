using WearCast.Api.Features.FixedProductColor.CreateFixedProductColor.DTOs;

namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductColor;

public class CreateFixedProductColorHandler : IRequestHandler<CreateFixedProductColorRequestDto, int>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;
    private readonly ImageService _imageService;

    public CreateFixedProductColorHandler(
        IRepository<Entities.FixedProduct.FixedProductColor> colorRepo,
        ImageService imageService)
    {
        _colorRepo = colorRepo;
        _imageService = imageService;
    }

    public async Task<int> Handle(CreateFixedProductColorRequestDto request, CancellationToken cancellationToken)
    {
        var imageUrl = await _imageService.UploadAsync(request.Image);

        var additionalImages = new List<Entities.FixedProduct.FixedProductImage>();
        if (request.AdditionalImages != null && request.AdditionalImages.Any())
        {
            var uploadTasks = request.AdditionalImages.Select(img => _imageService.UploadAsync(img));
            var urls = await Task.WhenAll(uploadTasks);

            additionalImages = urls.Select(url => new Entities.FixedProduct.FixedProductImage
            {
                ImageUrl = url
            }).ToList();
        }

        var mappedSizes = request.Sizes?.Select(s => new Entities.FixedProduct.FixedProductSize
        {
            Size = s.Size, 
            Quantity = s.Quantity
        }).ToList() ?? new List<Entities.FixedProduct.FixedProductSize>();

        var color = new Entities.FixedProduct.FixedProductColor
        {
            ProductId = request.ProductId,
            ColorName = request.ColorName,
            ColorCode = request.ColorCode,
            ImageUrl = imageUrl,
            Images = additionalImages,
            Sizes = mappedSizes
        };

        var result = await _colorRepo.CreateAsync(color);
        return result.Id;
    }
}