namespace WearCast.Api.Features.Drivers.CreateDriver
{
    [Route("api/drivers")]
    [ApiController]
    public class CreateDriverEndPoint(IMediator mediator) : ControllerBase
    {
        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateDriverRequest request, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
