using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.Sellers.SellerApplications.GetAllSellerApplications.DTOs
{
    public class GetAllSellerApplicationsRequestDTO : IRequest<Result<PagingViewModel<GetAllSellerApplicationsResponseDTO>>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public SortBy SortBy { get; set; } = SortBy.Newest;

        public string? SellerName { get; set; } = null;
        public string? SellerEmail { get; set; } = null;
        public string? ManagerFirstName { get; set; } = null;
        public string? ManagerLastName { get; set; } = null;
        public string? City { get; set; } = null;
        public Status? Status { get; set; } = null;
        public bool? IsEmailConfirmed { get; set; } = null;

        public DateTime? CreatedFrom { get; set; } = null;
        public DateTime? CreatedTo { get; set; } = null;
    }

    public class GetAllSellerApplicationsValidator : AbstractValidator<GetAllSellerApplicationsRequestDTO>
    {
        public GetAllSellerApplicationsValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0)
                .WithMessage("Page index must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.SortBy)
                .IsInEnum()
                .WithMessage("Invalid sort option.");

            RuleFor(x => x.SellerName)
                .MaximumLength(100).WithMessage("Seller name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.SellerName));

            RuleFor(x => x.SellerEmail)
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrWhiteSpace(x.SellerEmail));
           
            RuleFor(x => x.ManagerFirstName)
                .MaximumLength(100).WithMessage("Manager first name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.ManagerFirstName));

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("City name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.City));

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid Status.")
                .When(x => x.Status.HasValue);

            RuleFor(x => x.CreatedTo)
                .GreaterThanOrEqualTo(x => x.CreatedFrom)
                .WithMessage("CreatedTo date must be after or equal to CreatedFrom date.")
                .When(x => x.CreatedFrom.HasValue && x.CreatedTo.HasValue);
        }
    }
}

