using System.Text.Json.Serialization;

namespace WearCast.Api.Features.Drivers.ChangeDriverStatus.DTOs
{
    public class UpdateDriverStatusRequestDTO : IRequest<Result>
    {
        [JsonIgnore]
        public int DriverId { get; set; }
        [JsonIgnore]
        public string? UpdaterId { get; set; } = null;
        [JsonIgnore]
        public bool IsAdmin { get; set; } = false;
        public DriverStatus NewStatus { get; set; }
    }
    public class UpdateDriverStatusValidator : AbstractValidator<UpdateDriverStatusRequestDTO>
    {
        public UpdateDriverStatusValidator()
        {
            RuleFor(x => x.DriverId)
                .GreaterThan(0)
                .WithMessage("Driver ID must be valid.");

            RuleFor(x => x.NewStatus)
                .IsInEnum()
                .WithMessage("The provided status is not valid.");
        }
    }
}
