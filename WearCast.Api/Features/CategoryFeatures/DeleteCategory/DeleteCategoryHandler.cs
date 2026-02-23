namespace WearCast.Api.Features.CategoryFeatures.DeleteCategory;

public class DeleteCategoryHandler(IRepository<Category> categoryRepo)
    : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepo.GetAsync(c => c.Id == request.Id);
        if (category == null)
            return false;

        await categoryRepo.SoftDeleteAsync(request.Id);
        return true;
    }
}