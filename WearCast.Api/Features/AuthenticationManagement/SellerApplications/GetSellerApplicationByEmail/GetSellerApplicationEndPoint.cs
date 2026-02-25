namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.GetSellerApplicationByEmail
{
    [Route("api/seller-applications")]
    [ApiController]
    public class GetSellerApplicationEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{email}")]
        public async Task<IActionResult> Get([FromRoute] string email, CancellationToken cancellationToken)
        {
            var request = new GetSellerApplicationRequest(email);

            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
