using System.Text.Json.Serialization;

namespace WearCast.Api.Features.UpdateShipmentStatus.DTOs
{
    public class UpdateShipmentStatusRequestDTO : IRequest<UpdateShipmentStatusResponseDTO>
    {
        public int ShipmentId { get; set; }
        public ShipmentStatus NewStatus { get; set; }

        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;

        [JsonIgnore]
        public string UserRole { get; set; } = string.Empty;
    }
}
