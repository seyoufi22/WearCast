using System.ComponentModel.DataAnnotations;
using WearCast.Api.Common.Repository;
using WearCast.Api.Persistence.Services;
using static System.Net.Mime.MediaTypeNames;

namespace WearCast.Api.Features.CategoryFeatures.Commends
{
    public class CreateCategory
    {
        public class CreateCategoryCommand : IRequest<Category>
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public IFormFile Image { get; set; }
        }

        public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Category>
        {
            private readonly IRepository<Category> _categoryRepo;
            private readonly ImageService _imageService;

            public CreateCategoryHandler(IRepository<Category> categoryRepo, ImageService imageService)
            {
                _categoryRepo = categoryRepo;
                _imageService = imageService;
            }

            public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                var imageUrl = await _imageService.UploadFileAsync(request.Image);
                if (imageUrl.StartsWith("Invalid"))
                    return new Category { ImageUrl = imageUrl };
                var category = new Category
                {
                    Name = request.Name,
                    ImageUrl = imageUrl
                };

                await _categoryRepo.CreateAsync(category);

                return category;
            }
        }
    }
}