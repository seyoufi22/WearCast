namespace WearCast.Api.Features.Drivers.UpdateDriver
{
    public record UpdateDriverRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string NationalId,
        DeliveryVehicleType VehicleType,
        string? VehiclePlateNumber,
        AddressDto Address,
        int? ProvidedDriverId = null
        ) : IRequest<Result>;
}
