using WearCast.Api.Entities.FixedProduct;
using WearCast.Api.Features.FixedProductColor.CreateFixedProductColor.DTOs;
using WearCast.Api.Features.FixedProductColor.Errors;
namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductColor;

public class CreateFixedProductColorHandler : IRequestHandler<CreateFixedProductColorCommandDto, Result>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;
    private readonly ImageService _imageService;

    public CreateFixedProductColorHandler(
        IRepository<Entities.FixedProduct.FixedProductColor> colorRepo,
        ImageService imageService,
        IRepository<Entities.FixedProduct.FixedProduct> productRepo)
    {
        _colorRepo = colorRepo;
        _imageService = imageService;
        _productRepo = productRepo;
    }

    public async Task<Result> Handle(CreateFixedProductColorCommandDto command, CancellationToken cancellationToken)
    {
        var product = await _productRepo.GetAsync(p => p.Id == command.request.ProductId);

        if (product is null)
        {
            return Result.Failure(FixedProductColorErrors.ProductNotFound(command.request.ProductId));
        }

        if (product.SellerId != command.sellerId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }
        string normalizedCode = command.request.ColorCode.ToUpper().Trim();


        bool colorExists = await _colorRepo.Get().AnyAsync(c =>
            c.IsDeleted == false &&
            c.ProductId == command.request.ProductId &&
            c.ColorCode == normalizedCode);

        if (colorExists)
        {
            return Result.Failure(FixedProductColorErrors.DuplicateHexCode);
        }

        var mainImageValidation = _imageService.Validate(command.request.Image);
        if (!mainImageValidation.IsValid)
        {
            return Result.Failure(new Error("Product.MainImageInvalid", mainImageValidation.ErrorMessage, 400));
        }
        if (command.request.AdditionalImages is { Count: > 0 })
        {
            foreach (var img in command.request.AdditionalImages)
            {
                var additionalValidation = _imageService.Validate(img);
                if (!additionalValidation.IsValid)
                {
                    return Result.Failure(new Error("Product.AdditionalImageInvalid",
                        $"One Or More of the additional images is invalid: {additionalValidation.ErrorMessage}", 400));
                }
            }
        }

        string? mainUrl = await _imageService.UploadAsync(command.request.Image);
        if (string.IsNullOrEmpty(mainUrl))
        {
            return Result.Failure(FixedProductColorErrors.UploadFailed);
        }
        var additionalImageEntities = new List<FixedProductImage>();

        if (command.request.AdditionalImages is { Count: > 0 })
        {
            foreach (var img in command.request.AdditionalImages)
            {
                string? additionalUrl = await _imageService.UploadAsync(img);

                if (!string.IsNullOrEmpty(additionalUrl))
                {
                    additionalImageEntities.Add(new FixedProductImage
                    {
                        ImageUrl = additionalUrl,
                    });
                }
            }
        }
        

        var mappedSizes = command.request.Sizes.Select(s => new Entities.FixedProduct.FixedProductSize
        {
            Size = s.Size,
            Quantity = s.Quantity
        }).OrderBy(s => s.Size).ToList();

        var color = new Entities.FixedProduct.FixedProductColor
        {
            ProductId = command.request.ProductId,
            ColorName = command.request.ColorName.Trim(), 
            ColorCode = normalizedCode, 
            ImageUrl = mainUrl,
            Images = additionalImageEntities,
            Sizes = mappedSizes
        };

        var result = await _colorRepo.CreateAsync(color);

        return Result.Success();
    }
}