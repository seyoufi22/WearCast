using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace WearCast.Api.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<SellerManager> SellerManagers { get; set; }
    public DbSet<Factory> Factories { get; set; }
    public DbSet<FactoryManager> FactoryManagers { get; set; }
    public DbSet<ShippingCompany> ShippingCompanies { get; set; }
    public DbSet<ShippingCompanyManager> ShippingCompanyManagers { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SellerApplication> SellerApplications { get; set; }

    public DbSet<Entities.FixedProduct.FixedProduct> FixedProducts { get; set; }
    public DbSet<Entities.FixedProduct.FixedProductColor> FixedProductColors { get; set; }
    public DbSet<Entities.FixedProduct.FixedProductImage> FixedProductImages { get; set; }
    public DbSet<Entities.FixedProduct.FixedProductSize> FixedProductSizes { get; set; }

    public DbSet<DesignedProduct> DesignedProducts { get; set; }
    public DbSet<DesignedProductColor> DesignedProductColors { get; set; }
    public DbSet<DesignedProductImage> DesignedProductImages { get; set; }
    public DbSet<DesignAsset> DesignAssets { get; set; }
    public DbSet<DesignAssetCategory> DesignAssetCategories { get; set; }
    public DbSet<CustomerDesign> CustomerDesigns { get; set; }
    public DbSet<CustomerUploadedImage> CustomerUploadedImages { get; set; }
    public DbSet<DesignedProductSizeDetails> DesignedProductSizeDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var softDeletableEntities = modelBuilder.Model.GetEntityTypes()
        .Where(e => typeof(ISoftDeletable).IsAssignableFrom(e.ClrType) && e.BaseType == null);

        foreach (var entity in softDeletableEntities)
        {
            var parameter = Expression.Parameter(entity.ClrType, "e");
            var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
            var falseConstant = Expression.Constant(false);
            var condition = Expression.Equal(property, falseConstant);
            var lambda = Expression.Lambda(condition, parameter);

            modelBuilder.Entity(entity.ClrType).HasQueryFilter(lambda);
        }

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseModel>();

        foreach (var entityEntry in entries)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId()!;
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}