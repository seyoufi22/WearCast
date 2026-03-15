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
}
