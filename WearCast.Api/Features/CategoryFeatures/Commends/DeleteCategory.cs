using WearCast.Api.Common.Repository;
using WearCast.Api.Persistence.Services;

namespace WearCast.Api.Features.CategoryFeatures.Commends
{
    public class DeleteCategory
    {
        public record DeleteCategoryCommand(Guid Id) : IRequest<bool>;

        public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
        {
            private readonly IRepository<Category> _categoryRepo;
            private readonly ImageService _imageService;

            public DeleteCategoryHandler(IRepository<Category> categoryRepo,ImageService imageService)
            {
                _categoryRepo = categoryRepo;
                _imageService = imageService;
            }

            public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = await _categoryRepo.GetAsync(c => c.ID == request.Id);

                if (category == null)
                {
                    return false;
                }
                await _categoryRepo.SoftDeleteAsync(request.Id);

                return true;
            }
        }
    }

}
