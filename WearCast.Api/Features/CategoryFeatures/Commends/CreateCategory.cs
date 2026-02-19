using System.ComponentModel.DataAnnotations;
using WearCast.Api.Common.Repository;
using WearCast.Api.Common.Services;

namespace WearCast.Api.Features.CategoryFeatures.Commends
{
    public class CreateCategory
    {
        public class CreateCategoryCommand : IRequest<Category>
        {
            public string Name { get; set; }
            public IFormFile Image { get; set; }
        }
        public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
        {
            private readonly IRepository<Category> _categoryRepo;
            private readonly ImageService _imageService;

            public CreateCategoryCommandValidator(IRepository<Category> categoryRepo, ImageService imageService)
            {
                _categoryRepo = categoryRepo;
                _imageService = imageService;
                RuleFor(x => x.Name)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Category name is required.")
                    .Must(name => !string.IsNullOrWhiteSpace(name))
                    .WithMessage("Category name cannot be just spaces.")
                    .MustAsync(BeUniqueName)
                    .WithMessage("Category name already exists.");
                RuleFor(x => x.Image)
                    .Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("Image is required.")
                    .Must((command, file, context) =>
                    {
                        var result = _imageService.Validate(file);
                        if (!result.IsValid)
                            context.AddFailure(result.ErrorMessage);
                        return result.IsValid;
                    });
            }

            private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
            {
                var existing = await _categoryRepo
                    .GetAsync(c => c.Name.ToLower() == name.ToLower());
                return existing == null;
            }
        }

    }
}