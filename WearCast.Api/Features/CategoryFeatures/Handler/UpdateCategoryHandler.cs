using WearCast.Api.Common.Repository;
using WearCast.Api.Common.Services;
using static WearCast.Api.Features.CategoryFeatures.Commends.UpdateCategory;

namespace WearCast.Api.Features.CategoryFeatures.Handler
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, string>
    {
        private readonly IRepository<Category> _categoryRepo;
        private readonly ImageService _imageService;

        public UpdateCategoryHandler(IRepository<Category> categoryRepo, ImageService imageService)
        {
            _categoryRepo = categoryRepo;
            _imageService = imageService;
        }

        public async Task<string> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepo.GetAsync(c => c.Id == request.Id);

            if (category == null)
                return $"Category with ID {request.Id} not found";

            category.Name = request.Name;

            if (request.Image != null)
            {
                string url = await _imageService.UploadAsync(request.Image);
                if (url.StartsWith("Invalid"))
                    return url;
                await _imageService.DeleteAsync(category.ImageUrl);
                category.ImageUrl = url;
            }

            await _categoryRepo.UpdateAsync(category);
            return "";
        }
    }
}
