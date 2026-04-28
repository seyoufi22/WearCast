namespace WearCast.Api.Features.Sellers.SellerApplications.GetSellerApplicationById.DTOs
{
    public class GetSellerApplicationByIdRequestDTO : IRequest<Result<GetSellerApplicationByIdResponseDTO>>
    {
        public GetSellerApplicationByIdRequestDTO(int applicationId)
        {
            ApplicationId = applicationId;
        }

        public int ApplicationId { get; set; }
    }
    public class GetSellerApplicationByIdValidator : AbstractValidator<GetSellerApplicationByIdRequestDTO>
    {
        public GetSellerApplicationByIdValidator()
        {
            RuleFor(x => x.ApplicationId)
                .GreaterThan(0)
                .WithMessage("Application ID must be valid.");
        }
    }
}
