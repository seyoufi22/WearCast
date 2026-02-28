using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Drivers.GetAllDrivers.DTOs;
using WearCast.Api.Features.FixedProduct.GetFixedProductById.DTOs;

namespace WearCast.Api.Features.Drivers.GetAllDrivers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllDriversEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAllDriversEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetAllDriversRequestDTO(), cancellationToken);

            return Ok(result);
        }
    }
}
