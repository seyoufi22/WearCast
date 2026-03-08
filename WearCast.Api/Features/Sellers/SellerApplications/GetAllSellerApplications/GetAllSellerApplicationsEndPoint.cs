//namespace WearCast.Api.Features.Sellers.SellerApplication.GetAllSellerApplications
//{
//    [Route("api/seller-applications")]
//    [ApiController]
//    public class GetSellerApplicationEndPoint(IMediator mediator) : ControllerBase
//    {
//        private readonly IMediator _mediator = mediator;

//        [HttpGet]
//        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
//        {
//            var result = await mediator.Send(new GetSellerApplicationRequest(), cancellationToken);

//            return result.ToResponse();
//        }
//    }
//}
