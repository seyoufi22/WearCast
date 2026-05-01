namespace WearCast.Api.Features.Drivers.DeleteDriver.DTOs
{
    public class DeleteDriverRequestDTO : IRequest<Result>
    {
        public int DriverId { get; set; }
        public string ManagerId { get; set; } = string.Empty;
    }
}
