using WearCast.Api.Common.Repository;
using WearCast.Api.Common.Services;
using static WearCast.Api.Features.CategoryFeatures.Commends.CreateCategory;

namespace WearCast.Api.Features.CategoryFeatures.Handler
{
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
            var imageUrl = await _imageService.UploadAsync(request.Image);
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
