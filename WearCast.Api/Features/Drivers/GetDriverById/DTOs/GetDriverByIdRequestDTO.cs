
namespace WearCast.Api.Features.Drivers.GetDriverById.DTOs
{
    public class GetDriverByIdRequestDTO : IRequest<Result<GetDriverByIdResponseDTO>>
    {
        public GetDriverByIdRequestDTO(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
    public class GetDriverByIdValidator : AbstractValidator<GetDriverByIdRequestDTO>
    {
        public GetDriverByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Driver ID must be valid.");
        }
    }
}
