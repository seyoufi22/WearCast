using System.Text.Json.Serialization;

namespace WearCast.Api.Features.Drivers.GetShipmentsByDriverId.DTOs
{
    public class GetShipmentsByDriverIdRequestDTO : IRequest<Result<IEnumerable<GetShipmentsByDriverIdResponse>>>
    {
        [JsonIgnore]
        public int DriverId { get; set; }
    }
    public class GetShipmentsByDriverIdValidator : AbstractValidator<GetShipmentsByDriverIdRequestDTO>
    {
        public GetShipmentsByDriverIdValidator()
        {
            RuleFor(x => x.DriverId)
                .GreaterThan(0).WithMessage("Driver ID must be valid.");
        }
    }
}
