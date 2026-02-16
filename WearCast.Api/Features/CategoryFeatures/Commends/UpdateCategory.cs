using System.ComponentModel.DataAnnotations;
using WearCast.Api.Common.Repository;
using WearCast.Api.Persistence.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WearCast.Api.Features.CategoryFeatures.Commends
{
    public class UpdateCategory
    {
        public class UpdateCategoryCommand : IRequest<string>
        {
            [Required]
            public Guid Id { get; set; }

            [Required]
            public string Name { get; set; }

            public IFormFile? Image { get; set; }
        }

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
                var category = await _categoryRepo.GetAsync(c => c.ID == request.Id);

                if (category == null)
                    return $"Category with ID {request.Id} not found";

                category.Name = request.Name;

                if (request.Image != null)
                {
                    string url = await _imageService.UploadFileAsync(request.Image);
                    if (url.StartsWith("Invalid"))
                        return url;
                    await _imageService.DeleteFileAsync(category.ImageUrl);
                    category.ImageUrl = url;
                }

                await _categoryRepo.UpdateAsync(category);
                return "";
            }
        }
    }
}