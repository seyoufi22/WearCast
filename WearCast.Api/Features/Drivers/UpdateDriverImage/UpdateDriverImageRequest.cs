namespace WearCast.Api.Features.Drivers.UpdateDriverImage
{
    public record UpdateDriverImageRequest(IFormFile NewImage, int? ProvidedDriverId = null) : IRequest<Result>;
}
