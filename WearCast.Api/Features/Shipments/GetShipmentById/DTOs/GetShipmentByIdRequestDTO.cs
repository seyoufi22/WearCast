namespace WearCast.Api.Features.Shipments.GetShipmentById.DTOs
{
    public class GetShipmentByIdRequestDTO: IRequest<Result<GetShipmentByIdResponseDTO>>
    {
        public GetShipmentByIdRequestDTO(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
    public class GetShipmentByIdValidator : AbstractValidator<GetShipmentByIdRequestDTO>
    {
        public GetShipmentByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Shipment ID must be valid.");
        }
    }
}
