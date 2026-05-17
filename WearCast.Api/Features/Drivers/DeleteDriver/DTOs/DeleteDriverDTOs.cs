using System.Text.Json.Serialization;

namespace WearCast.Api.Features.Drivers.DeleteDriver.DTOs
{
    public class DeleteDriverRequestDTO : IRequest<Result>
    {
        public int DriverId { get; set; }
        public string? UpdaterId { get; set; } = null;
    }

    public class DeleteDriverValidator : AbstractValidator<DeleteDriverRequestDTO>
    {
        public DeleteDriverValidator()
        {
            RuleFor(x => x.DriverId)
                .GreaterThan(0)
                .WithMessage("Driver ID must be valid.");
        }
    }
}
