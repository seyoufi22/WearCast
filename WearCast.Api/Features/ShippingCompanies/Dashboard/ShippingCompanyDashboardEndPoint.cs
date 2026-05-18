using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.ShippingCompanies.Dashboard.DTOs;

namespace WearCast.Api.Features.ShippingCompanies.Dashboard
{
    [Route("api/ShippingCompany/Dashboard")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipping Company Profile")]
    public class ShippingCompanyDashboardEndPoint(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new ShippingCompanyDashboardRequestDTO(), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}
