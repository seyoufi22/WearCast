using WearCast.Api.Features.FixedProduct.Errors;
using WearCast.Api.Features.FixedProduct.DeleteFixedProduct.DTOs;
// Ensure the correct using directive for AuthErrors is included if it's in a different namespace

namespace WearCast.Api.Features.FixedProduct.DeleteFixedProduct.Command;

public class DeleteFixedProductHandler : IRequestHandler<DeleteFixedProductRequest, Result>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _productColorRepo;

    public DeleteFixedProductHandler(
        IRepository<Entities.FixedProduct.FixedProduct> productRepo,
        IRepository<Entities.FixedProduct.FixedProductColor> productColorRepo)
    {
        _productRepo = productRepo;
        _productColorRepo = productColorRepo;
    }

    public async Task<Result> Handle(DeleteFixedProductRequest request, CancellationToken cancellationToken)
    {
        // 1. Fetch the product and pass the CancellationToken
        var product = await _productRepo.GetAsync(
            p => p.Id == request.Id,
            useNoTracking: true);

        if (product == null)
        {
            return Result.Failure(FixedProductErrors.ProductNotFound);
        }

        // 2. Check authorization
        if (!request.isAdminRequest && product.SellerId != request.SellerId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        // 3. Soft delete the main product 
        await _productRepo.SoftDeleteAsync(product.Id);

        // 4. Soft delete all related colors using a single fast query
        await _productColorRepo.SoftDeleteByConditionAsync(
            pc => pc.ProductId == request.Id,
            cancellationToken);

        // Uncomment the following line if you use a Unit of Work pattern without auto-save
        // await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}