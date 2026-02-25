using WearCast.Api.Common.Repository;
using WearCast.Api.Abstractions;
using WearCast.Api.Features.FixedProduct.Errors;
using WearCast.Api.Features.FixedProduct.UpdateFixedProduct.DTOs;
using WearCast.Api.Entities;
using WearCast.Api.Entities.FixedProduct;

namespace WearCast.Api.Features.FixedProduct.UpdateFixedProduct.Command;

public class UpdateFixedProductHandler : IRequestHandler<UpdateFixedProductRequestDto, Result<UpdateFixedProductResponseDto>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;
    private readonly IRepository<Category> _categoryRepo;

    public UpdateFixedProductHandler(
        IRepository<Entities.FixedProduct.FixedProduct> productRepo,
        IRepository<Category> categoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<Result<UpdateFixedProductResponseDto>> Handle(UpdateFixedProductRequestDto request, CancellationToken cancellationToken)
    {
        var product = await _productRepo.GetAsync(p => p.Id == request.Id);
        
        if (product == null)
        {
            return Result.Failure<UpdateFixedProductResponseDto>(FixedProductErrors.ProductNotFound);
        }

        var categoryExists = await _categoryRepo.GetAsync(c => c.Id == request.CategoryId, useNoTracking: true);
        if (categoryExists == null)
        {
            return Result.Failure<UpdateFixedProductResponseDto>(FixedProductErrors.CategoryNotFound);
        }

        var existingProductWithSameName = await _productRepo.GetAsync(p => p.Name == request.Name && p.Id != request.Id, useNoTracking: true);
        if (existingProductWithSameName != null)
        {
            return Result.Failure<UpdateFixedProductResponseDto>(FixedProductErrors.DuplicateName);
        }

        product.CategoryId = request.CategoryId;
        product.Name = request.Name;
        product.Price = request.Price;
        product.Description = request.Description;
        product.TargetAudience = request.TargetAudience;

        product.SizeDetails.Clear();
        foreach (var sd in request.SizeDetails)
        {
            product.SizeDetails.Add(new ProductSizeDetail
            {
                Size = sd.Size,
                A = sd.A,
                B = sd.B,
                C = sd.C
            });
        }
        
        await _productRepo.UpdateAsync(product);

        return Result.Success(new UpdateFixedProductResponseDto(product.Id, product.Name));
    }
}
