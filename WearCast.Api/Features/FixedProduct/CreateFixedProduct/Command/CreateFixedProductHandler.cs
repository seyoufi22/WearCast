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
    private readonly IRepository<Entities.Category> _categoryRepo;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateFixedProductHandler(
        IRepository<Entities.FixedProduct.FixedProduct> productRepo,
        IRepository<Entities.Category> categoryRepo,
        UserManager<ApplicationUser> userManager)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _userManager = userManager;
    }

    public async Task<Result<CreateFixedProductResponseDto>> Handle(CreateFixedProductRequestDto request, CancellationToken cancellationToken)
    {
        var errors = new List<Error>();

        var categoryExists = await _categoryRepo.GetAsync(c => c.Id == request.CategoryId, useNoTracking: true);
        if (categoryExists == null)
        {
            errors.Add(FixedProductErrors.CategoryNotFound(request.CategoryId));
        }

        var userExists = await _userManager.FindByIdAsync(request.CreatedById);
        if (userExists == null)
        {
            errors.Add(FixedProductErrors.UserNotFound(request.CreatedById));
        }

        var existingProduct = await _productRepo.GetAsync(p => p.Name == request.Name, useNoTracking: true);
        if (existingProduct != null)
        {
            errors.Add(FixedProductErrors.DuplicateName(request.Name));
        }

        if (errors.Any())
        {
            var combinedMessage = string.Join("; ", errors.Select(e => e.Message));
            return Result.Failure<CreateFixedProductResponseDto>(
                new Error("FixedProduct.ValidationFailed", combinedMessage, StatusCodes.Status400BadRequest));
        }

        var product = new Entities.FixedProduct.FixedProduct
        {
            CategoryId = request.CategoryId,
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            DressStyle = request.DressStyle,
            TargetAudience = request.TargetAudience,
            CreatedById = request.CreatedById,
            SellerId = request.SellerId,
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
