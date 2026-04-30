namespace WearCast.Api.Features.Factories.DeleteFactory
{
    [Route("api/factories")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.SuperAdmin)]
    [Tags("Factory Profile")]
    public class DeleteFactoryEndpoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFactory(
            [FromRoute] int id,
            [FromBody] DeleteFactoryBody body,
            CancellationToken cancellationToken)
        {
            var request = new DeleteFactoryRequest(id, body.Reason);
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}