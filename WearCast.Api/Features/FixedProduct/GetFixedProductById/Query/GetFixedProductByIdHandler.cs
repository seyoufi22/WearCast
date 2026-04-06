using WearCast.Api.Common.Repository;
using WearCast.Api.Abstractions;
using WearCast.Api.Features.FixedProduct.Errors;
using WearCast.Api.Features.FixedProduct.GetFixedProductById.DTOs;

namespace WearCast.Api.Features.FixedProduct.GetFixedProductById.Query;

public class GetFixedProductByIdHandler : IRequestHandler<GetFixedProductByIdQuery, Result<GetFixedProductByIdResponseDto>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;

    public GetFixedProductByIdHandler(IRepository<Entities.FixedProduct.FixedProduct> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Result<GetFixedProductByIdResponseDto>> Handle(GetFixedProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepo.GetAsync(p => p.Id == request.Id, useNoTracking: true);
        
        if (product == null)
        {
            return Result.Failure<GetFixedProductByIdResponseDto>(FixedProductErrors.ProductNotFound);
        }

        var response = new GetFixedProductByIdResponseDto
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            TargetAudience = product.TargetAudience.ToString(),
            SellerId = product.SellerId,
            SizeDetails = product.SizeDetails.Select(sd => new ProductSizeDetailResponseDto
            {
                Size = sd.Size.ToString(),
                A = sd.A,
                B = sd.B,
                C = sd.C
            }).ToList()
        };

        return Result.Success(response);
    }
}
