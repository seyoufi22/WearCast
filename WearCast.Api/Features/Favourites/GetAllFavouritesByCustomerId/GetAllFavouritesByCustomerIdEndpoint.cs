using WearCast.Api.Features.Favourites.GetAllFavouritesByCustomerId.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WearCast.Api.Features.Favourites.GetAllFavouritesByCustomerId;

[ApiController]
[Route("api/Favourites/GetAllByCustomerId")]
[Tags("Favourites")]
public class GetAllFavouritesByCustomerIdEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetAllFavouritesByCustomerIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllByCustomerId([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100, CancellationToken cancellationToken = default)
    {
        var customerId = HttpContext.User.GetCustomerId();
        if (customerId == null)
            return Unauthorized("User is not a valid customer.");

        var result = await _sender.Send(new GetAllFavouritesByCustomerIdQuery(customerId.Value, pageIndex, pageSize), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
