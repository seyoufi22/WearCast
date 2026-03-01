using WearCast.Api.Common.Repository;
using WearCast.Api.Abstractions;
using WearCast.Api.Features.FixedProduct.GetAllFixedProducts.DTOs;
using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProducts.Query;

public class GetAllFixedProductsHandler : IRequestHandler<GetAllFixedProductsQuery, Result<PagingViewModel<GetAllFixedProductsResponseDto>>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;

    public GetAllFixedProductsHandler(IRepository<Entities.FixedProduct.FixedProduct> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Result<PagingViewModel<GetAllFixedProductsResponseDto>>> Handle(GetAllFixedProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _productRepo.Get().Include(p => p.SizeDetails).AsNoTracking();

        var projectedQuery = query.Select(product => new GetAllFixedProductsResponseDto
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            TargetAudience = product.TargetAudience.ToString(),
            SizeDetails = product.SizeDetails.Select(sd => new ProductSizeDetailGetAllResponseDto
            {
                Size = sd.Size.ToString(),
                A = sd.A,
                B = sd.B,
                C = sd.C
            }).ToList()
        });

        var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

        return Result.Success(pagedResult);
    }
}
