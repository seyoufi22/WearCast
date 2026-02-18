using System.ComponentModel.DataAnnotations;
using WearCast.Api.Common.Repository;
using WearCast.Api.Persistence.Services;

namespace WearCast.Api.Features.CategoryFeatures.Commends
{
    public class UpdateCategory
    {
        public class UpdateCategoryCommand : IRequest<string>
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public IFormFile? Image { get; set; }
        }

        public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
        {
            private readonly IRepository<Category> _categoryRepo;
            private readonly ImageService _imageService;

            public UpdateCategoryCommandValidator(IRepository<Category> categoryRepo, ImageService imageService)
            {
                _categoryRepo = categoryRepo;
                _imageService = imageService;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("Invalid category ID.")
                    .MustAsync(async (id, cancellationToken) =>
                    {
                        var existing = await _categoryRepo.GetAsync(c => c.Id == id);
                        return existing != null;
                    }).WithMessage("Category not found.");

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Category name is required.")
                    .Must(name => !string.IsNullOrWhiteSpace(name))
                    .WithMessage("Category name cannot be just spaces.")
                    .MustAsync(BeUniqueName)
                    .WithMessage("Category name already exists.");

                RuleFor(x => x.Image)
                    .Must((command, file, context) =>
                    {
                        if (file == null) return true; 

                        var result = _imageService.Validate(file);
                        if (!result.IsValid)
                            context.AddFailure(result.ErrorMessage);
                        return result.IsValid;
                    });
            }

            private async Task<bool> BeUniqueName(UpdateCategoryCommand command, string name, ValidationContext<UpdateCategoryCommand> context, CancellationToken cancellationToken)
            {
                var existing = await _categoryRepo
                    .GetAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != command.Id);
                return existing == null;
            }
        }
    }
}