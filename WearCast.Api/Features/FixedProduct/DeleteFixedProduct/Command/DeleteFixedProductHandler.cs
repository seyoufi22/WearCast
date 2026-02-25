using WearCast.Api.Common.Repository;
using WearCast.Api.Abstractions;
using WearCast.Api.Features.FixedProduct.Errors;
using WearCast.Api.Features.FixedProduct.DeleteFixedProduct.DTOs;

namespace WearCast.Api.Features.FixedProduct.DeleteFixedProduct.Command;

public class DeleteFixedProductHandler : IRequestHandler<DeleteFixedProductRequest, Result>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;

    public DeleteFixedProductHandler(IRepository<Entities.FixedProduct.FixedProduct> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Result> Handle(DeleteFixedProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productRepo.GetAsync(p => p.Id == request.Id, useNoTracking: true);
        
        if (product == null)
        {
            return Result.Failure(FixedProductErrors.ProductNotFound);
        }

        await _productRepo.SoftDeleteAsync(product.Id);

        return Result.Success();
    }
}
