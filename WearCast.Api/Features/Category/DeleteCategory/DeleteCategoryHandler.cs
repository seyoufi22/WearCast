using WearCast.Api.Features.Category.DeleteCategory.DTOs;

namespace WearCast.Api.Features.Category.DeleteCategory;

public class DeleteCategoryHandler(IRepository<Entities.Category> categoryRepo)
    : IRequestHandler<DeleteCategoryRequestDto, Result>
{
    public async Task<Result> Handle(DeleteCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var category = await categoryRepo.GetAsync(c => c.Id == request.Id);
        if (category == null)
            return Result.Failure(new Error("Category.NotFound",$"Category with ID {request.Id} was not found.",404));

        await categoryRepo.SoftDeleteAsync(request.Id);
        return Result.Success();
    }
}