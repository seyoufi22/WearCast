//namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.AddCustomerDesign
//{
//    [Route("api/customers/me/designs")]
//    [ApiController]
//    [Authorize]
//    public class AddCustomerDesignEndPoint(IMediator mediator) : ControllerBase
//    {
//        private readonly IMediator _mediator = mediator;
//        [HttpPost]
//        public async Task<IActionResult> Add([FromBody] AddCustomerDesignRequest request, CancellationToken cancellationToken)
//        {
//            var result = await _mediator.Send(request, cancellationToken);

//            return result.ToResponse();
//        }
//    }
//}
