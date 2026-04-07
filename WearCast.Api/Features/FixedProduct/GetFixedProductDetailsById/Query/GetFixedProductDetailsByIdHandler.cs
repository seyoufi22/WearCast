using WearCast.Api.Common.Repository;
using WearCast.Api.Abstractions;
using WearCast.Api.Features.FixedProduct.Errors;
using WearCast.Api.Features.FixedProduct.GetFixedProductDetailsById.DTOs;
using WearCast.Api.Features.FixedProduct.GetFixedProductById.DTOs;

namespace WearCast.Api.Features.FixedProduct.GetFixedProductDetailsById.Query;

public class GetFixedProductDetailsByIdHandler : IRequestHandler<GetFixedProductDetailsByIdQuery, Result<GetFixedProductDetailsByIdResponseDto>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;

    public GetFixedProductDetailsByIdHandler(IRepository<Entities.FixedProduct.FixedProduct> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Result<GetFixedProductDetailsByIdResponseDto>> Handle(GetFixedProductDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepo.Get()
            .Where(p => p.Id == request.Id && !p.IsDeleted)
            .Include(p => p.Category)
            .Include(p => p.Colors.Where(c => !c.IsDeleted))
                .ThenInclude(c => c.Images)
            .Include(p => p.Colors.Where(c => !c.IsDeleted))
                .ThenInclude(c => c.Sizes)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null)
        {
            return Result.Failure<GetFixedProductDetailsByIdResponseDto>(FixedProductErrors.ProductNotFound);
        }

        var response = new GetFixedProductDetailsByIdResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            TargetAudience = product.TargetAudience.ToString(),
            SellerId = product.SellerId,
            Category = new ProductDetailsCategoryDto
            {
                Id = product.Category.Id,
                Name = product.Category.Name,
                ImageUrl = product.Category.ImageUrl
            },
            Colors = product.Colors?.Select(c => new ProductDetailsColorDto
            {
                Id = c.Id,
                ColorName = c.ColorName,
                ColorCode = c.ColorCode,
                ImageUrl = c.ImageUrl,
                Images = c.Images?.Select(i => new ProductDetailsImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl
                }).ToList() ?? new(),
                AvailableSizes = c.Sizes?.Select(s => new ProductDetailsSizeDto
                {
                    Size = s.Size.ToString(),
                    Quantity = s.Quantity
                }).ToList() ?? new()
            }).ToList() ?? new(),
            SizeDetails = product.SizeDetails?.Select(sd => new ProductSizeDetailResponseDto
            {
                Size = sd.Size.ToString(),
                A = sd.A,
                B = sd.B,
                C = sd.C
            }).ToList() ?? new()
        };

        return Result.Success(response);
    }
}
