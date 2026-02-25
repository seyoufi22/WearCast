using WearCast.Api.Common.Repository;
using WearCast.Api.Features.FixedProduct.CreateProduct.DTOs;
using WearCast.Api.Abstractions;
using WearCast.Api.Features.FixedProduct.Errors;
using Microsoft.AspNetCore.Identity;
using WearCast.Api.Entities.Identity;
using WearCast.Api.Entities;
using WearCast.Api.Entities.FixedProduct;

namespace WearCast.Api.Features.FixedProduct.CreateProduct.Command;

public class CreateFixedProductHandler : IRequestHandler<CreateFixedProductRequestDto, Result<CreateFixedProductResponseDto>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;
    private readonly IRepository<Category> _categoryRepo;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateFixedProductHandler(
        IRepository<Entities.FixedProduct.FixedProduct> productRepo,
        IRepository<Category> categoryRepo,
        UserManager<ApplicationUser> userManager)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _userManager = userManager;
    }

    public async Task<Result<CreateFixedProductResponseDto>> Handle(CreateFixedProductRequestDto request, CancellationToken cancellationToken)
    {
        var categoryExists = await _categoryRepo.GetAsync(c => c.Id == request.CategoryId, useNoTracking: true);
        if (categoryExists == null)
        {
            return Result.Failure<CreateFixedProductResponseDto>(FixedProductErrors.CategoryNotFound);
        }

        var userExists = await _userManager.FindByIdAsync(request.CreatedById);
        if (userExists == null)
        {
            return Result.Failure<CreateFixedProductResponseDto>(FixedProductErrors.UserNotFound);
        }

        var existingProduct = await _productRepo.GetAsync(p => p.Name == request.Name, useNoTracking: true);
        if (existingProduct != null)
        {
            return Result.Failure<CreateFixedProductResponseDto>(FixedProductErrors.DuplicateName);
        }

        var product = new Entities.FixedProduct.FixedProduct
        {
            CategoryId = request.CategoryId,
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            TargetAudience = request.TargetAudience,
            CreatedById = request.CreatedById,
            SizeDetails = request.SizeDetails.Select(sd => new ProductSizeDetail
            {
                Size = sd.Size,
                A = sd.A,
                B = sd.B,
                C = sd.C
            }).ToList()
        };
        
        await _productRepo.CreateAsync(product);

        return Result.Success(new CreateFixedProductResponseDto(product.Id, product.Name));
    }
}
