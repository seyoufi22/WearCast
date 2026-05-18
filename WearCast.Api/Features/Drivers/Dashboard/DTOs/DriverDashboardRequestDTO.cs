using System.Text.Json.Serialization;

namespace WearCast.Api.Features.Drivers.Dashboard.DTOs
{
    public class DriverDashboardRequestDTO : IRequest<Result<DriverDashboardResponseDTO>>
    {
        [JsonIgnore]
        public int DriverId { get; set; }
    }
    public class DriverDashboardValidator : AbstractValidator<DriverDashboardRequestDTO>
    {
        public DriverDashboardValidator()
        {
            RuleFor(x => x.DriverId)
                .GreaterThan(0).WithMessage("Invalid Driver ID.");
        }
    }
}
