using Bogus;
using Microsoft.AspNetCore.Identity;
using WearCast.Api.Common.Consts;
using WearCast.Api.Common.Enums;
using WearCast.Api.Common.ValueObjects;
using WearCast.Api.Entities;
using WearCast.Api.Entities.BusinessActors;
using WearCast.Api.Entities.DesignedProducts;
using WearCast.Api.Entities.FixedProduct;
using WearCast.Api.Entities.Identity;
using WearCast.Api.Entities.Order;
using WearCast.Api.Entities.Shipping;
using WearCast.Api.Persistence;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Persistence;

public class DbSeeder
{
    private readonly ApplicationDbContext dbContext;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<ApplicationRole> roleManager;
    private readonly IPasswordHasher<SellerApplication> _passwordHasher;
    private readonly Faker _faker = new();
    private string _adminId = string.Empty;
    private ApplicationUser _adminUser = default!;

    public DbSeeder(
        ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IPasswordHasher<SellerApplication> passwordHasher)
    {
        this.dbContext = dbContext;
        this.userManager = userManager;
        this.roleManager = roleManager;
        _passwordHasher = passwordHasher;
    }

    private readonly string[] _fashionImages = 
    [
        "https://images.unsplash.com/photo-1521572267360-ee0c2909d518",
        "https://images.unsplash.com/photo-1562157873-818bc0726f68",
        "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a",
        "https://images.unsplash.com/photo-1539008886428-40cd9827dc05",
        "https://images.unsplash.com/photo-1595777457583-95e059d581b8",
        "https://images.unsplash.com/photo-1551028719-00167b16eac5",
        "https://images.unsplash.com/photo-1591047139829-d91aecb6caea",
        "https://images.unsplash.com/photo-1542291026-7eec264c2744",
        "https://images.unsplash.com/photo-1491553895911-0055eca6402d",
        "https://images.unsplash.com/photo-1523275335684-37898b6baf30",
        "https://images.unsplash.com/photo-1549298916-b41d501d3772",
        "https://images.unsplash.com/photo-1525966222134-fcfa99bafb75",
        "https://images.unsplash.com/photo-1512436991641-6745cdb1723f",
        "https://images.unsplash.com/photo-1505740420928-5e560c06d30e",
        "https://images.unsplash.com/photo-1572635196237-14b3f281503f",
        "https://images.unsplash.com/photo-1445205170230-053b830c6050",
        "https://images.unsplash.com/photo-1483985988355-763728e1935b",
        "https://images.unsplash.com/photo-1490481651871-ab68de25d43d",
        "https://images.unsplash.com/photo-1479064566237-6b700eb1513a",
        "https://images.unsplash.com/photo-1434389677669-e08b4cac3105"
    ];

    private readonly string[] _profileImages = 
    [
        "https://images.unsplash.com/photo-1534528741775-53994a69daeb",
        "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d",
        "https://images.unsplash.com/photo-1500648767791-00dcc994a43e",
        "https://images.unsplash.com/photo-1494790108377-be9c29b29330",
        "https://images.unsplash.com/photo-1524504388940-b1c1722653e1",
        "https://images.unsplash.com/photo-1539571696357-5a69c17a67c6",
        "https://images.unsplash.com/photo-1488426862026-3ee34a7d66df",
        "https://images.unsplash.com/photo-1531427186611-ecfd6d936c79",
        "https://images.unsplash.com/photo-1506794778202-cad84cf45f1d",
        "https://images.unsplash.com/photo-1544005313-94ddf0286df2",
        "https://images.unsplash.com/photo-1517841905240-472988babdf9",
        "https://images.unsplash.com/photo-1501196354995-cbb51c65aaea"
    ];

    private readonly Dictionary<string, string[]> _categoryImageMap = new()
    {
        { "T-Shirts", new[] { "https://images.unsplash.com/photo-1521572267360-ee0c2909d518", "https://images.unsplash.com/photo-1562157873-818bc0726f68", "https://images.unsplash.com/photo-1576566582414-b1c4b7863548", "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a" } },
        { "Jeans", new[] { "https://images.unsplash.com/photo-1542272604-787c3835535d", "https://images.unsplash.com/photo-1541099649105-f69ad21f3246", "https://images.unsplash.com/photo-1514316454349-750a7fd3da3a", "https://images.unsplash.com/photo-1604176354204-926873ff349c" } },
        { "Hoodies", new[] { "https://images.unsplash.com/photo-1556821840-3a63f95609a7", "https://images.unsplash.com/photo-1513373319109-eb154073eb0b", "https://images.unsplash.com/photo-1559551409-dadc959f76b8", "https://images.unsplash.com/photo-1620799140408-edc6dcb6d633" } },
        { "Suits", new[] { "https://images.unsplash.com/photo-1594932224828-b4b05a832fe2", "https://images.unsplash.com/photo-1593032465175-481ac7f402a1", "https://images.unsplash.com/photo-1507679799987-c73779587ccf", "https://images.unsplash.com/photo-1598808503742-dd34bd6543c0" } },
        { "Dresses", new[] { "https://images.unsplash.com/photo-1585487000160-6ebcfceb0d03", "https://images.unsplash.com/photo-1595777457583-95e059d581b8", "https://images.unsplash.com/photo-1612336307429-8a898d10e223", "https://images.unsplash.com/photo-1515372039744-b8f02a3ae446" } },
        { "Activewear", new[] { "https://images.unsplash.com/photo-1518310383802-640c2de311b2", "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b", "https://images.unsplash.com/photo-1583454110551-21f2fa2ec617", "https://images.unsplash.com/photo-1538805060514-97d9cc17730c" } },
        { "Footwear", new[] { "https://images.unsplash.com/photo-1549298916-b41d501d3772", "https://images.unsplash.com/photo-1542291026-7eec264c2744", "https://images.unsplash.com/photo-1560769629-975ec94e6a86", "https://images.unsplash.com/photo-1595950653106-6c9ebd614d3a" } },
        { "Accessories", new[] { "https://images.unsplash.com/photo-1523275335684-37898b6baf30", "https://images.unsplash.com/photo-1505740420928-5e560c06d30e", "https://images.unsplash.com/photo-1572635196237-14b3f281503f", "https://images.unsplash.com/photo-1584917865442-de89df76afd3" } }
    };

    private string GetRandomFashionImage() => _faker.PickRandom(_fashionImages);
    private string GetRandomProfileImage() => _faker.PickRandom(_profileImages);
    private string GetCategoryImage(string categoryName) => 
        _categoryImageMap.TryGetValue(categoryName, out var images) ? _faker.PickRandom(images) : GetRandomFashionImage();

    public async Task SeedAsync()
    {
        try
        {
            Console.WriteLine("[Seeder] Ensuring database is created...");
            await dbContext.Database.EnsureCreatedAsync();
            
            await ClearDatabaseAsync();

            // 1. Roles
            await SeedRolesAsync();

            // 2. Admin User
            _adminUser = await SeedAdminUserAsync();
            _adminId = _adminUser.Id;

            // 3. Categories
            var categories = await SeedCategoriesAsync();
            var assetCategories = await SeedDesignAssetCategoriesAsync();

            // 4. Business Actors
            var customers = await SeedCustomersAsync(500);
            var sellers = await SeedSellersAsync(15);
            var factories = await SeedFactoriesAsync(10);
            var shippingCompanies = await SeedShippingCompaniesAsync(5);
            
            await SeedDriversAsync(shippingCompanies, 15);

            // 5. Products
            var fixedProducts = await SeedFixedProductsAsync(sellers, categories, 100);
            var designedProducts = await SeedDesignedProductsAsync(factories, categories, 200);

            // 6. Assets
            await SeedDesignAssetsAsync(assetCategories, 50);

            // 7. Interactions
            await SeedFavouritesAsync(customers, fixedProducts, 300);
            await SeedCartItemsAsync(customers, fixedProducts, 150);
            await SeedCustomerDesignsAsync(customers, designedProducts, 100);
            await SeedCustomerUploadedImagesAsync(customers);

            // 8. Product Extras
            await SeedDesignedProductImagesAsync(designedProducts);
            await SeedDesignedProductReviewsAsync(customers, designedProducts);

            // 9. Orders & Shipments
            var orders = await SeedOrdersAsync(customers, sellers, factories, 500);
            await SeedShipmentsAsync(orders, shippingCompanies, 450);

            // 10. Activity Logs (Crucial for Recommendations)
            await SeedActivityLogsAsync(customers, fixedProducts, designedProducts, 30000);

            // 11. Seller Applications
            await SeedSellerApplicationsAsync(dbContext);

            Console.WriteLine("[Seeder] Seeding Complete!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Seeder] FATAL ERROR: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"[Seeder] INNER ERROR: {ex.InnerException.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }

    private async Task SeedRolesAsync()
    {
        var roles = new List<(string Id, string Name)>
        {
            (DefaultRoles.SuperAdminRoleId, DefaultRoles.SuperAdmin),
            (DefaultRoles.CustomerRoleId, DefaultRoles.Customer),
            (DefaultRoles.SellerManagerRoleId, DefaultRoles.SellerManager),
            (DefaultRoles.FactoryManagerRoleId, DefaultRoles.FactoryManager),
            (DefaultRoles.ShippingCompanyManagerRoleId, DefaultRoles.ShippingCompanyManager),
            (DefaultRoles.DriverRoleId, DefaultRoles.Driver)
        };

        foreach (var (id, name) in roles)
        {
            if (await roleManager.FindByIdAsync(id) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole { Id = id, Name = name, NormalizedName = name.ToUpper() });
            }
        }
    }

    private async Task<ApplicationUser> SeedAdminUserAsync()
    {
        var admin = await userManager.FindByEmailAsync("admin@wearcast.com");
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                Id = DefaultRoles.SuperAdminRoleId,
                UserName = "admin@wearcast.com",
                Email = "admin@wearcast.com",
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Admin",
                PhoneNumber = "01000000001",
                PhoneNumberConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            await userManager.AddToRoleAsync(admin, DefaultRoles.SuperAdmin);
        }
        return admin;
    }

    private async Task<List<Category>> SeedCategoriesAsync()
    {
        if (await dbContext.Categories.AnyAsync()) return await dbContext.Categories.ToListAsync();

        var categories = _categoryImageMap.Keys
            .Select(name => new Category
            {
                Name = name,
                CreatedBy = _adminUser,
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                ImageUrl = GetCategoryImage(name)
            }).ToList();

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
        return categories;
    }

    private async Task<List<DesignAssetCategory>> SeedDesignAssetCategoriesAsync()
    {
        if (await dbContext.DesignAssetCategories.AnyAsync()) return await dbContext.DesignAssetCategories.ToListAsync();

        var categories = new List<string> { "Logos", "Patterns", "Illustrations", "Text Styles", "Vintage" }
            .Select(name => new DesignAssetCategory
            {
                Name = name,
                CreatedBy = _adminUser,
                CreatedOn = DateTime.UtcNow
            }).ToList();

        await dbContext.DesignAssetCategories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
        return categories;
    }

    private async Task<List<Customer>> SeedCustomersAsync(int count)
    {
        if (await dbContext.Customers.CountAsync() >= count) return await dbContext.Customers.ToListAsync();

        var customers = new List<Customer>();
        for (int i = 0; i < count; i++)
        {
            var user = new ApplicationUser
            {
                UserName = _faker.Internet.UserName() + i,
                Email = _faker.Internet.Email(),
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                PhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, "User123!");
            await userManager.AddToRoleAsync(user, DefaultRoles.Customer);

            var customer = new Customer
            {
                ApplicationUser = user,
                ProfileImageUrl = GetRandomProfileImage(),
                Address = GenerateAddress()
            };
            customers.Add(customer);
        }
        await dbContext.Customers.AddRangeAsync(customers);
        await dbContext.SaveChangesAsync();
        return customers;
    }

    private async Task<List<Seller>> SeedSellersAsync(int count)
    {
        if (await dbContext.Sellers.CountAsync() >= count) return await dbContext.Sellers.ToListAsync();

        var sellers = new List<Seller>();
        for (int i = 0; i < count; i++)
        {
            var user = new ApplicationUser
            {
                UserName = "seller" + i,
                Email = $"seller{i}@test.com",
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                PhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, "Seller123!");
            await userManager.AddToRoleAsync(user, DefaultRoles.SellerManager);

            var seller = new Seller
            {
                Name = _faker.Company.CompanyName() + " Boutique",
                Email = user.Email,
                PhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                Description = _faker.Company.CatchPhrase(),
                LogoUrl = "https://images.unsplash.com/photo-1599305090748-394e7bd9b569",
                Address = GenerateAddress(),
                CommercialRegisterNumber = _faker.Random.Number(1000000, 9999999).ToString(),
                TaxIdNumber = _faker.Random.Number(1000000, 9999999).ToString()
            };
            sellers.Add(seller);
            
            await dbContext.Sellers.AddAsync(seller);
            await dbContext.SaveChangesAsync();

            await dbContext.SellerManagers.AddAsync(new SellerManager { ApplicationUser = user, Seller = seller });
        }
        await dbContext.SaveChangesAsync();
        return sellers;
    }

    private async Task<List<Factory>> SeedFactoriesAsync(int count)
    {
        if (await dbContext.Factories.CountAsync() >= count) return await dbContext.Factories.ToListAsync();

        var factories = new List<Factory>();
        for (int i = 0; i < count; i++)
        {
            var user = new ApplicationUser
            {
                UserName = "factory" + i,
                Email = $"factory{i}@test.com",
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                PhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, "Factory123!");
            await userManager.AddToRoleAsync(user, DefaultRoles.FactoryManager);

            var factory = new Factory
            {
                Name = _faker.Company.CompanyName() + " Lab",
                Email = user.Email,
                PhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                Description = _faker.Company.Bs(),
                LogoUrl = "https://images.unsplash.com/photo-1541185933-ef5d8ed016c2",
                Address = GenerateAddress(),
                CommercialRegisterNumber = _faker.Random.Number(1000000, 9999999).ToString(),
                TaxIdNumber = _faker.Random.Number(1000000, 9999999).ToString()
            };
            factories.Add(factory);
            
            await dbContext.Factories.AddAsync(factory);
            await dbContext.SaveChangesAsync();

            await dbContext.FactoryManagers.AddAsync(new FactoryManager { ApplicationUser = user, Factory = factory });
        }
        await dbContext.SaveChangesAsync();
        return factories;
    }

    private async Task<List<ShippingCompany>> SeedShippingCompaniesAsync(int count)
    {
        if (await dbContext.ShippingCompanies.CountAsync() >= count) return await dbContext.ShippingCompanies.ToListAsync();

        var companies = new List<ShippingCompany>();
        for (int i = 0; i < count; i++)
        {
            var user = new ApplicationUser
            {
                UserName = "shipco" + i,
                Email = $"shipco{i}@test.com",
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                PhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, "ShipCo123!");
            await userManager.AddToRoleAsync(user, DefaultRoles.ShippingCompanyManager);

            var company = new ShippingCompany
            {
                Name = _faker.Company.CompanyName() + " Logistics",
                Email = user.Email,
                PhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                Description = _faker.Lorem.Sentence(),
                LogoUrl = "https://images.unsplash.com/photo-1566576721346-d4a3b4eaad5b",
                Address = GenerateAddress(),
                CommercialRegisterNumber = _faker.Random.Number(1000000, 9999999).ToString(),
                TaxIdNumber = _faker.Random.Number(1000000, 9999999).ToString(),
                DeliveryFee = _faker.Random.Decimal(20, 100)
            };
            companies.Add(company);
            
            await dbContext.ShippingCompanies.AddAsync(company);
            await dbContext.SaveChangesAsync();

            await dbContext.ShippingCompanyManagers.AddAsync(new ShippingCompanyManager
            {
                ApplicationUser = user,
                ShippingCompany = company
            });
        }
        await dbContext.SaveChangesAsync();
        return companies;
    }

    private async Task SeedDriversAsync(List<ShippingCompany> companies, int count)
    {
        if (await dbContext.Drivers.AnyAsync()) return;

        for (int i = 0; i < count; i++)
        {
            var company = _faker.PickRandom(companies);
            var user = new ApplicationUser
            {
                UserName = "driver" + i,
                Email = $"driver{i}@test.com",
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                PhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, "Driver123!");
            await userManager.AddToRoleAsync(user, DefaultRoles.Driver);

            var driver = new Driver
            {
                UserId = user.Id,
                ShippingCompanyId = company.Id,
                VehicleType = (DeliveryVehicleType)_faker.Random.Int(1, 4),
                VehiclePlateNumber = _faker.Random.Replace("??-####"),
                NationalId = _faker.Random.Replace("##############"),
                Address = GenerateAddress(),
                Status = _faker.Random.Bool(0.8f) ? DriverStatus.Available : DriverStatus.NotAvailable,
                ProfileImageUrl = GetRandomProfileImage()
            };
            await dbContext.Drivers.AddAsync(driver);
        }
        await dbContext.SaveChangesAsync();
    }

    private async Task<List<FixedProduct>> SeedFixedProductsAsync(List<Seller> sellers, List<Category> categories, int count)
    {
        if (await dbContext.FixedProducts.CountAsync() >= count) return await dbContext.FixedProducts.ToListAsync();

        var products = new List<FixedProduct>();
        var sizes = Enum.GetValues<Size>().ToList();

        for (int i = 0; i < count; i++)
        {
            var seller = _faker.PickRandom(sellers);
            var category = _faker.PickRandom(categories);

            var product = new FixedProduct
            {
                Name = _faker.Commerce.ProductName(),
                Description = _faker.Commerce.ProductDescription(),
                Price = _faker.Random.Decimal(50, 1000),
                TargetAudience = (TargetAudience)_faker.Random.Int(1, 4),
                DressStyle = (DressStyle)_faker.Random.Int(1, 5),
                CategoryId = category.Id,
                SellerId = seller.Id,
                CreatedById = _adminId,
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                SizeDetails = sizes.Select(s => new ProductSizeDetail 
                { 
                    Size = s, 
                    A = _faker.Random.Decimal(30, 65), 
                    B = _faker.Random.Decimal(40, 85), 
                    C = _faker.Random.Decimal(10, 30) 
                }).ToList()
            };
            products.Add(product);
            await dbContext.FixedProducts.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Add Colors & Sizes
            var colorCount = _faker.Random.Int(1, 4);
            for (int c = 0; c < colorCount; c++)
            {
                var productColor = new FixedProductColor
                {
                    ProductId = product.Id,
                    ColorName = _faker.Commerce.Color(),
                    ColorCode = _faker.Internet.Color(),
                    ImageUrl = GetCategoryImage(category.Name),
                    CreatedById = _adminId,
                    Sizes = sizes.Select(s => new FixedProductSize { Size = s, Quantity = _faker.Random.Int(0, 50) }).ToList()
                };
                await dbContext.FixedProductColors.AddAsync(productColor);
                await dbContext.SaveChangesAsync();

                // Add Images
                int imgCount = _faker.Random.Int(2, 4);
                for (int img = 0; img < imgCount; img++)
                {
                    await dbContext.FixedProductImages.AddAsync(new FixedProductImage 
                    { 
                        ProductColorId = productColor.Id, 
                        ImageUrl = GetCategoryImage(category.Name),
                        CreatedById = _adminId 
                    });
                }
            }
        }
        await dbContext.SaveChangesAsync();
        return products;
    }

    private async Task<List<DesignedProduct>> SeedDesignedProductsAsync(List<Factory> factories, List<Category> categories, int count)
    {
        if (await dbContext.DesignedProducts.CountAsync() >= count) return await dbContext.DesignedProducts.ToListAsync();

        var products = new List<DesignedProduct>();
        for (int i = 0; i < count; i++)
        {
            var factory = _faker.PickRandom(factories);
            var category = _faker.PickRandom(categories);

            var product = new DesignedProduct
            {
                Name = "Design Base " + _faker.Commerce.ProductName(),
                Description = _faker.Commerce.ProductDescription(),
                Price = _faker.Random.Decimal(100, 1500),
                TargetAudience = (TargetAudience)_faker.Random.Int(1, 4),
                DressStyle = (DressStyle)_faker.Random.Int(1, 5),
                CategoryId = category.Id,
                FactoryId = factory.Id,
                CreatedById = _adminId,
                IsActive = true,
                CanvasWidth = 1024,
                CanvasHeight = 1024,
                SalesCount = _faker.Random.Int(0, 100),
                AverageRating = _faker.Random.Decimal(3.5m, 5.0m),
                ReviewCount = _faker.Random.Int(10, 200)
            };
            products.Add(product);
            await dbContext.DesignedProducts.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Add Colors
            var color = new DesignedProductColor
            {
                DesignedProductId = product.Id,
                Name = _faker.Commerce.Color(),
                HexCode = _faker.Internet.Color(),
                MainImageUrl = GetCategoryImage(category.Name),
                CreatedById = _adminId
            };
            await dbContext.DesignedProductColors.AddAsync(color);
            await dbContext.SaveChangesAsync();

            // Set Default Color
            product.DefaultColorId = color.Id;
            dbContext.DesignedProducts.Update(product);
            await dbContext.SaveChangesAsync();

            // Add Size Details
            foreach (var sz in Enum.GetValues<Size>())
            {
                await dbContext.DesignedProductSizeDetails.AddAsync(new DesignedProductSizeDetails
                {
                    DesignedProductId = product.Id,
                    Size = sz,
                    A = _faker.Random.Decimal(30, 60),
                    B = _faker.Random.Decimal(40, 80),
                    C = _faker.Random.Decimal(10, 25)
                    // No CreatedById: DesignedProductSizeDetails is ISoftDeletable, not BaseModel
                });
            }
        }
        await dbContext.SaveChangesAsync();
        return products;
    }

    private async Task SeedDesignAssetsAsync(List<DesignAssetCategory> categories, int count)
    {
        if (await dbContext.DesignAssets.AnyAsync()) return;

        var assets = new List<DesignAsset>();
        for (int i = 0; i < count; i++)
        {
            assets.Add(new DesignAsset
            {
                Name = _faker.Commerce.ProductName() + " Asset",
                ImageUrl = GetRandomFashionImage(),
                DesignAssetCategoryId = _faker.PickRandom(categories).Id,
                CreatedById = _adminId,
                WidthPx = 512,
                HeightPx = 512
            });
        }
        await dbContext.DesignAssets.AddRangeAsync(assets);
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedFavouritesAsync(List<Customer> customers, List<FixedProduct> products, int count)
    {
        if (await dbContext.Favourites.AnyAsync()) return;

        var existing = new HashSet<(int, int)>();
        for (int i = 0; i < count; i++)
        {
            var customer = _faker.PickRandom(customers);
            var product = _faker.PickRandom(products);
            var color = await dbContext.FixedProductColors.FirstOrDefaultAsync(c => c.ProductId == product.Id);
            
            if (color != null && !existing.Contains((customer.Id, color.Id)))
            {
                await dbContext.Favourites.AddAsync(new Favourite { CustomerId = customer.Id, FixedProductColorId = color.Id });
                existing.Add((customer.Id, color.Id));
            }
        }
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedCartItemsAsync(List<Customer> customers, List<FixedProduct> products, int count)
    {
        if (await dbContext.CartItems.AnyAsync()) return;

        var sizes = Enum.GetValues<Size>().ToList();
        for (int i = 0; i < count; i++)
        {
            var customer = _faker.PickRandom(customers);
            var product = _faker.PickRandom(products);
            var color = await dbContext.FixedProductColors
                .Include(c => c.Sizes)
                .FirstOrDefaultAsync(c => c.ProductId == product.Id);
            if (color != null)
            {
                var cartItem = new CartItem
                {
                    CustomerId = customer.Id,
                    FixedColorId = color.Id,
                    CreatedById = customer.UserId,
                    CreatedOn = DateTime.UtcNow
                };
                // Add a size that actually has stock in the color
                var availableSize = color.Sizes.FirstOrDefault(s => s.Quantity > 0);
                var chosenSize = availableSize?.Size ?? _faker.PickRandom(sizes);
                cartItem.Sizes.Add(new FixedProductSize { Size = chosenSize, Quantity = _faker.Random.Int(1, 3) });
                await dbContext.CartItems.AddAsync(cartItem);
            }
        }
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedCustomerDesignsAsync(List<Customer> customers, List<DesignedProduct> baseProducts, int count)
    {
        if (await dbContext.CustomerDesigns.AnyAsync()) return;

        for (int i = 0; i < count; i++)
        {
            var customer = _faker.PickRandom(customers);
            var baseProduct = _faker.PickRandom(baseProducts);
            var color = await dbContext.DesignedProductColors.FirstOrDefaultAsync(c => c.DesignedProductId == baseProduct.Id);
            if (color != null)
            {
                var categoryName = baseProduct.Category?.Name ?? (await dbContext.Categories.FindAsync(baseProduct.CategoryId))?.Name ?? "T-Shirts";
                
                await dbContext.CustomerDesigns.AddAsync(new CustomerDesign
                {
                    Name = _faker.Commerce.ProductName() + " Design",
                    CustomerId = customer.Id,
                    DesignedProductId = baseProduct.Id,
                    DesignedProductColorId = color.Id,
                    ViewDesignsJson = "{}",
                    FrontImageUrl = GetCategoryImage(categoryName),
                    BackImageUrl = GetCategoryImage(categoryName),
                    TotalPrice = baseProduct.Price,
                    AssetCount = _faker.Random.Int(0, 5),
                    CreatedById = customer.UserId
                });
            }
        }
        await dbContext.SaveChangesAsync();
    }

    private async Task<List<Order>> SeedOrdersAsync(List<Customer> customers, List<Seller> sellers, List<Factory> factories, int count)
    {
        if (await dbContext.Orders.AnyAsync()) return await dbContext.Orders.ToListAsync();

        var orders = new List<Order>();
        var allFixedColors = await dbContext.FixedProductColors
            .Include(c => c.Product)
            .ToListAsync();
        
        var allCustomerDesigns = await dbContext.CustomerDesigns
            .Include(d => d.DesignedProduct)
            .Include(d => d.DesignedProductColor)
            .ToListAsync();

        var statuses = Enum.GetValues<OrderStatus>().ToList();
        var sizes = Enum.GetValues<Size>().ToList();

        for (int i = 0; i < count; i++)
        {
            var customer = _faker.PickRandom(customers);
            bool isFactoryOrder = _faker.Random.Bool() && allCustomerDesigns.Any();
            var orderDate = DateTime.UtcNow.AddDays(-_faker.Random.Int(1, 180));
            
            var order = new Order
            {
                CustomerId = customer.Id,
                CreatedOn = orderDate,
                Status = _faker.PickRandom(new[] { OrderStatus.Pending, OrderStatus.Paid, OrderStatus.Ready, OrderStatus.PickedUp, OrderStatus.Cancelled, OrderStatus.Paid, OrderStatus.PickedUp }),
                RecipientName = _faker.Name.FullName(),
                RecipientPhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                RecipientAdditionalPhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                ShippingAddress = GenerateAddress(),
                PickUpAddress = GenerateAddress(),
                StripeSessionId = "cs_test_" + _faker.Random.AlphaNumeric(24),
                StripePaymentIntentId = "pi_test_" + _faker.Random.AlphaNumeric(24),
                CreatedById = customer.UserId
            };

            if (isFactoryOrder)
            {
                var design = _faker.PickRandom(allCustomerDesigns);
                order.FactoryId = design.DesignedProduct.FactoryId;
            }
            else
            {
                var seller = _faker.PickRandom(sellers);
                order.SellerId = seller.Id;
            }

            decimal totalAmount = 0;
            int itemCount = _faker.Random.Int(1, 3);

            if (isFactoryOrder)
            {
                for (int j = 0; j < itemCount; j++)
                {
                    var design = _faker.PickRandom(allCustomerDesigns);
                    var orderItem = new CustomerDesignedOrderItem
                    {
                        CustomerDesignId = design.Id,
                        DesignedProductId = design.DesignedProductId,
                        ProductName = design.DesignedProduct.Name,
                        ColorName = design.DesignedProductColor.Name,
                        FrontImageUrl = design.FrontImageUrl,
                        BackImageUrl = design.BackImageUrl,
                        LeftImageUrl = design.LeftImageUrl,
                        RightImageUrl = design.RightImageUrl,
                        ViewDesignsJson = design.ViewDesignsJson,
                        SizeName = _faker.PickRandom(sizes).ToString().Replace("_", ""),
                        Quantity = _faker.Random.Int(1, 2),
                        UnitPrice = design.TotalPrice,
                        CreatedById = customer.UserId
                    };
                    totalAmount += orderItem.UnitPrice * orderItem.Quantity;
                    order.DesignedProductItems.Add(orderItem);
                }
            }
            else
            {
                for (int j = 0; j < itemCount; j++)
                {
                    var color = _faker.PickRandom(allFixedColors);
                    var orderItem = new FixedProductOrderItem
                    {
                        FixedColorId = color.Id,
                        ProductName = color.Product.Name,
                        ColorName = color.ColorName,
                        ImageUrl = color.ImageUrl,
                        SizeName = _faker.PickRandom(sizes).ToString().Replace("_", ""),
                        Quantity = _faker.Random.Int(1, 5),
                        UnitPrice = color.Product.Price,
                        CreatedById = customer.UserId
                    };
                    totalAmount += orderItem.UnitPrice * orderItem.Quantity;
                    order.FixedProductItems.Add(orderItem);
                }
            }

            order.TotalAmount = totalAmount;
            orders.Add(order);
        }
        
        await dbContext.Orders.AddRangeAsync(orders);
        await dbContext.SaveChangesAsync();
        return orders;
    }

    private async Task SeedShipmentsAsync(List<Order> orders, List<ShippingCompany> companies, int count)
    {
        if (await dbContext.Shipments.AnyAsync()) return;

        var allDrivers = await dbContext.Drivers.ToListAsync();
        
        // Filter orders that could realistically have shipments
        var eligibleOrders = orders
            .Where(o => o.Status != OrderStatus.Pending && o.Status != OrderStatus.Cancelled)
            .ToList();

        for (int i = 0; i < Math.Min(count, eligibleOrders.Count); i++)
        {
            var order = eligibleOrders[i];
            var company = _faker.PickRandom(companies);
            var driversForCompany = allDrivers.Where(d => d.ShippingCompanyId == company.Id).ToList();

            if (order.ShipmentId == null)
            {
                // Better distribution for more realistic dashboard data
                var statusRoll = _faker.Random.Int(1, 100);
                ShipmentStatus status;
                
                if (statusRoll <= 50) status = ShipmentStatus.Delivered;
                else if (statusRoll <= 70) status = ShipmentStatus.OutForDelivery;
                else if (statusRoll <= 85) status = ShipmentStatus.PickingUp;
                else if (statusRoll <= 93) status = ShipmentStatus.Assigned;
                else if (statusRoll <= 97) status = ShipmentStatus.Unassigned;
                else status = ShipmentStatus.Pending;

                var baseDate = order.CreatedOn;

                var shipment = new Shipment
                {
                    ShippingCompanyId = company.Id,
                    ShipmentStatus = status,
                    CustomerId = order.CustomerId,
                    DeliveryAddress = order.ShippingAddress ?? GenerateAddress(),
                    Price = company.DeliveryFee,
                    DeliveryCode = _faker.Random.Number(1000, 9999).ToString(),
                    // Assign driver if status is Assigned or higher
                    DriverId = (status >= ShipmentStatus.Assigned && driversForCompany.Any()) 
                        ? _faker.PickRandom(driversForCompany).Id : null,
                    CreatedById = _adminId,
                    ReadyForPickupAt = baseDate.AddHours(_faker.Random.Int(2, 12)),
                };

                // Logical timeline based on status
                if (status >= ShipmentStatus.PickingUp)
                    shipment.TripStartedAt = shipment.ReadyForPickupAt?.AddHours(_faker.Random.Int(1, 4));
                
                if (status >= ShipmentStatus.OutForDelivery)
                    shipment.OutForDeliveryAt = shipment.TripStartedAt?.AddHours(_faker.Random.Int(2, 6));

                if (status == ShipmentStatus.Delivered)
                {
                    shipment.DeliveredAt = shipment.OutForDeliveryAt?.AddHours(_faker.Random.Int(1, 8));
                    order.Status = OrderStatus.PickedUp; // Mark order as completed
                }

                shipment.Orders.Add(order);
                await dbContext.Shipments.AddAsync(shipment);
                await dbContext.SaveChangesAsync();
                
                order.ShipmentId = shipment.Id;
            }
        }
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedActivityLogsAsync(List<Customer> customers, List<FixedProduct> fixedProducts, List<DesignedProduct> designedProducts, int count)
    {
        Console.WriteLine("[Seeder] Generating Realistic Activity Logs...");
        
        // Ensure Categories are in memory for names
        var categoryMap = await dbContext.Categories.ToDictionaryAsync(c => c.Id, c => c.Name);

        var logs = new List<UserActivityLog>();
        var random = new Random();

        for (int i = 0; i < count; i++)
        {
            var customer = _faker.PickRandom(customers);
            var eventType = _faker.PickRandom(new[] { "filter", "click", "view", "addToCart", "purchase" });
            var timestamp = DateTime.UtcNow.AddDays(-_faker.Random.Int(0, 180));

            object payload;

            if (eventType == "filter")
            {
                payload = new
                {
                    eventType = "filter",
                    userId = customer.UserId,
                    timestamp = timestamp.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"),
                    filters = new
                    {
                        searchKey = _faker.Random.Bool() ? _faker.Commerce.ProductAdjective() : null,
                        minPrice = _faker.Random.Bool() ? (decimal?)_faker.Random.Number(50, 200) : null,
                        maxPrice = _faker.Random.Bool() ? (decimal?)_faker.Random.Number(800, 2000) : null,
                        targetAudience = GenerateRandomAudienceList(),
                        dressStyle = _faker.Random.Bool() ? _faker.PickRandom<DressStyle>().ToString() : null,
                        categoryName = _faker.Random.Bool() ? _faker.PickRandom(categoryMap.Values.ToList()) : null,
                        sellerId = (string?)null
                    }
                };
            }
            else if (eventType == "purchase")
            {
                var purchaseItems = new List<object>();
                int itemCount = _faker.Random.Int(1, 3);
                for (int j = 0; j < itemCount; j++)
                {
                    var product = _faker.Random.Bool() ? (object)_faker.PickRandom(fixedProducts) : (object)_faker.PickRandom(designedProducts);
                    purchaseItems.Add(MapProductToDetails(product, categoryMap));
                }

                payload = new
                {
                    eventType = "purchase",
                    userId = customer.UserId,
                    timestamp = timestamp.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    products = purchaseItems
                };
            }
            else // click, view, addToCart
            {
                var product = _faker.Random.Bool() ? (object)_faker.PickRandom(fixedProducts) : (object)_faker.PickRandom(designedProducts);
                payload = new
                {
                    eventType = eventType,
                    userId = customer.UserId,
                    timestamp = timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"),
                    productDetails = MapProductToDetails(product, categoryMap)
                };
            }

            logs.Add(new UserActivityLog
            {
                UserId = customer.UserId,
                Payload = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                CreatedAt = timestamp
            });

            if (logs.Count >= 1000)
            {
                await dbContext.UserActivityLogs.AddRangeAsync(logs);
                await dbContext.SaveChangesAsync();
                logs.Clear();
                Console.Write(".");
            }
        }

        await dbContext.UserActivityLogs.AddRangeAsync(logs);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("\n[Seeder] Activity Logs Seeded Successfully.");
    }

    private async Task SeedSellerApplicationsAsync(ApplicationDbContext dbContext)
    {
        if (await dbContext.SellerApplications.AnyAsync()) return;

        var applications = new List<SellerApplication>();
        for (int i = 0; i < 10; i++)
        {
            var firstName = _faker.Name.FirstName();
            var lastName = _faker.Name.LastName();
            var email = _faker.Internet.Email(firstName, lastName);

            var application = new SellerApplication
            {
                ManagerFirstName = firstName,
                ManagerLastName = lastName,
                ManagerEmail = email,
                ManagerPhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                SellerName = _faker.Company.CompanyName(),
                SellerEmail = _faker.Internet.Email(),
                SellerPhoneNumber = _faker.Phone.PhoneNumber("01#########"),
                CommercialRegisterNumber = _faker.Random.Number(100000, 999999).ToString(),
                TaxIdNumber = _faker.Random.Number(100000, 999999).ToString(),
                Description = _faker.Company.CatchPhrase(),
                LogoUrl = _faker.Image.PlaceholderUrl(200, 200, "Seller"),
                SellerAddress = GenerateAddress(),
                ManagerEmailConfirmed = _faker.Random.Bool(),
                Status = _faker.PickRandom<Status>(),
                CreatedOn = _faker.Date.Past(1)
            };

            application.ManagerPasswordHash = _passwordHasher.HashPassword(application, "P@ssword123");
            applications.Add(application);
        }

        await dbContext.SellerApplications.AddRangeAsync(applications);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("[Seeder] Seller Applications Seeded Successfully.");
    }

    private async Task SeedDesignedProductImagesAsync(List<DesignedProduct> products)
    {
        if (await dbContext.DesignedProductImages.AnyAsync()) return;
        var colors = await dbContext.DesignedProductColors.ToListAsync();
        var images = new List<DesignedProductImage>();
        foreach (var color in colors)
        {
            foreach (var side in Enum.GetValues<ViewSide>())
            {
                images.Add(new DesignedProductImage
                {
                    DesignedProductColorId = color.Id,
                    ViewSide = (ViewSide)side,
                    ImageUrl = GetRandomFashionImage(),
                    CreatedById = _adminId
                });
            }
        }
        await dbContext.DesignedProductImages.AddRangeAsync(images);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("[Seeder] Designed Product Images Seeded Successfully.");
    }

    private async Task SeedDesignedProductReviewsAsync(List<Customer> customers, List<DesignedProduct> products)
    {
        if (await dbContext.DesignedProductReviews.AnyAsync()) return;
        var reviews = new List<DesignedProductReview>();
        foreach (var product in products)
        {
            int reviewCount = _faker.Random.Int(1, Math.Min(10, customers.Count));
            var reviewers = _faker.Random.ListItems(customers, reviewCount);
            foreach (var customer in reviewers)
            {
                reviews.Add(new DesignedProductReview
                {
                    CustomerId = customer.Id,
                    DesignedProductId = product.Id,
                    Rating = _faker.Random.Int(3, 5),
                    Comment = _faker.Rant.Review(),
                    CreatedById = customer.UserId
                });
            }
        }
        await dbContext.DesignedProductReviews.AddRangeAsync(reviews);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("[Seeder] Designed Product Reviews Seeded Successfully.");
    }

    private async Task SeedCustomerUploadedImagesAsync(List<Customer> customers)
    {
        if (await dbContext.CustomerUploadedImages.AnyAsync()) return;
        var uploadedImages = new List<CustomerUploadedImage>();
        foreach (var customer in customers)
        {
            int imageCount = _faker.Random.Int(0, 5);
            for (int i = 0; i < imageCount; i++)
            {
                uploadedImages.Add(new CustomerUploadedImage
                {
                    CustomerId = customer.Id,
                    ImageUrl = GetRandomFashionImage(),
                    CreatedById = customer.UserId
                });
            }
        }
        await dbContext.CustomerUploadedImages.AddRangeAsync(uploadedImages);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("[Seeder] Customer Uploaded Images Seeded Successfully.");
    }

    private object MapProductToDetails(object product, Dictionary<int, string> categoryMap)
    {
        if (product is FixedProduct fp)
        {
            return new
            {
                productId = "FIX_" + fp.Id,
                price = (double)fp.Price,
                targetAudience = new[] { fp.TargetAudience.ToString() },
                dressStyle = fp.DressStyle.ToString(),
                categoryName = categoryMap.GetValueOrDefault(fp.CategoryId, "Unknown"),
                sellerId = fp.SellerId.ToString()
            };
        }
        else
        {
            var dp = (DesignedProduct)product;
            return new
            {
                productId = "DES_" + dp.Id,
                price = (double)dp.Price,
                targetAudience = new[] { dp.TargetAudience.ToString() },
                dressStyle = dp.DressStyle.ToString(),
                categoryName = categoryMap.GetValueOrDefault(dp.CategoryId, "Unknown"),
                sellerId = (string?)null,
                averageRating = (double)dp.AverageRating,
                reviewCount = dp.ReviewCount
            };
        }
    }

    private List<string> GenerateRandomAudienceList()
    {
        var audiences = Enum.GetValues<TargetAudience>().Select(a => a.ToString()).ToList();
        return _faker.Random.ListItems(audiences, _faker.Random.Int(1, 2)).ToList();
    }

    private async Task ClearDatabaseAsync()
    {
        Console.WriteLine("[Seeder] Clearing existing database records...");

        // Order matters for Foreign Key constraints
        await dbContext.UserActivityLogs.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.CustomerDesignedOrderItems.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.FixedProductOrderItems.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.Orders.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.Shipments.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.CustomerDesigns.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.CartItems.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.Favourites.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.CustomerUploadedImages.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.DesignedProductReviews.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.DesignedProductImages.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.DesignedProductSizeDetails.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.DesignedProducts.IgnoreQueryFilters().ExecuteUpdateAsync(s => s.SetProperty(p => p.DefaultColorId, (int?)null));
        await dbContext.DesignedProductColors.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.DesignedProducts.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.FixedProductImages.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.FixedProductColors.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.FixedProducts.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.DesignAssets.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.DesignAssetCategories.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.Drivers.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.ShippingCompanyManagers.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.FactoryManagers.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.SellerManagers.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.ShippingCompanies.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.Factories.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.Sellers.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.Customers.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.Categories.IgnoreQueryFilters().ExecuteDeleteAsync();
        await dbContext.SellerApplications.IgnoreQueryFilters().ExecuteDeleteAsync();

        // Identity tables
        await dbContext.UserRoles.ExecuteDeleteAsync();
        await dbContext.UserClaims.ExecuteDeleteAsync();
        await dbContext.UserLogins.ExecuteDeleteAsync();
        await dbContext.UserTokens.ExecuteDeleteAsync();
        await dbContext.RoleClaims.ExecuteDeleteAsync();
        await dbContext.Users.ExecuteDeleteAsync();
        await dbContext.Roles.ExecuteDeleteAsync();

        Console.WriteLine("[Seeder] Database cleared successfully.");
    }

    private Address GenerateAddress() => new()
    {
        State = _faker.Address.State(),
        City = _faker.Address.City(),
        Street = _faker.Address.StreetName(),
        BuildingNumber = _faker.Random.Number(1, 999).ToString()
    };
}