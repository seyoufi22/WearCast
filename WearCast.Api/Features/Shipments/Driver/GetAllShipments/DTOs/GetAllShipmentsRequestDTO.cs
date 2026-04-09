using WearCast.Api.Features.Drivers.GetAllDrivers.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.GetAllShipments.DTOs
{
    public class GetAllShipmentsRequestDTO: IRequest<Result<IEnumerable<GetAllShipmentsResponseDTO>>>
    {
    }
}
