namespace WearCast.Api.Features.Shipments.Customer.GetShipmentById.DTOs
{
    public class GetCustomerShipmentByIdRequestDTO : IRequest<Result<GetCustomerShipmentByIdResponseDTO>>
    {
        public GetCustomerShipmentByIdRequestDTO(int shipmentid, int customerid)
        {
            ShipmentId = shipmentid;
            CustomerId = customerid;
        }

        public int ShipmentId { get; set; }
        public int CustomerId { get; set; }
    }
    public class GetCustomerShipmentByIdValidator : AbstractValidator<GetCustomerShipmentByIdRequestDTO>
    {
        public GetCustomerShipmentByIdValidator()
        {
            RuleFor(x => x.ShipmentId)
                .GreaterThan(0)
                .WithMessage("Shipment ID must be valid.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Customer ID must be valid.");
        }
    }
}
