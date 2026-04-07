namespace WearCast.Api.Features.Factories.UpdateFactoryImage
{
    public record UpdateFactoryImageRequest(IFormFile NewLogo, int? ProvidedFactoryId = null) : IRequest<Result>;
}
