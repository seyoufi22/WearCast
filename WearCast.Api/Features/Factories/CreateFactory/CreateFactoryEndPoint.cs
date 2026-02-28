namespace WearCast.Api.Features.Factories.CreateFactory
{
    [Route("api/factories")]
    [ApiController]
    public class CreateFactoryEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateFactoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return result.ToResponse();
        }
    }
}
