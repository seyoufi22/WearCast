using WearCast.Api.Common.Consts;
using WearCast.Api.Common.Repository;
using WearCast.Api.Features.FixedProduct.CreateProduct.DTOs;
using WearCast.Api.Features.FixedProduct.CreateFixedProduct.Notifications;
using WearCast.Api.Abstractions;
using WearCast.Api.Features.FixedProduct.Errors;
using Microsoft.AspNetCore.Identity;
using WearCast.Api.Entities.Identity;
using WearCast.Api.Entities;
using WearCast.Api.Entities.FixedProduct;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.FixedProduct.CreateProduct.Command;

public class CreateFixedProductHandler : IRequestHandler<CreateFixedProductRequestDto, Result<CreateFixedProductResponseDto>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;
    private readonly IRepository<Entities.Category> _categoryRepo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;

    public CreateFixedProductHandler(
        IRepository<Entities.FixedProduct.FixedProduct> productRepo,
        IRepository<Entities.Category> categoryRepo,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        IMediator mediator)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _userManager = userManager;
        _context = context;
        _mediator = mediator;
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
        
        var catalogAdminUserIds = await (from adminUser in _context.Users
                                         join ur in _context.UserRoles on adminUser.Id equals ur.UserId
                                         join r in _context.Roles on ur.RoleId equals r.Id
                                         where r.Name == DefaultRoles.CatalogAdmin && !adminUser.IsDeleted
                                         select adminUser.Id).ToListAsync(cancellationToken);
        if (catalogAdminUserIds.Any())
        {
            var notificationEvent = new NewProductEvent(
                RecipientIds: catalogAdminUserIds,
                ProductId: product.Id,
                ProductName: product.Name,
                ProductType: "Fixed Product");
            await _mediator.Publish(notificationEvent, cancellationToken);
        }

        return Result.Success(new CreateFixedProductResponseDto(product.Id, product.Name));
    }
}
