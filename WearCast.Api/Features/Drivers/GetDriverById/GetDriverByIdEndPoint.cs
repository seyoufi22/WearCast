using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WearCast.Api.Features.Drivers.GetDriverById.DTOs;
using WearCast.Api.Features.FixedProduct.GetFixedProductById.DTOs;

namespace WearCast.Api.Features.Drivers.GetDriverById
{
    [ApiController]
    [Tags("Drivers")]
    [Route("api/Drivers/GetById")]
    public class GetDriverByIdEndPoint : ControllerBase
    {
        private readonly ISender _sender;
        public GetDriverByIdEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetDriverByIdRequestDTO(id), cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}
