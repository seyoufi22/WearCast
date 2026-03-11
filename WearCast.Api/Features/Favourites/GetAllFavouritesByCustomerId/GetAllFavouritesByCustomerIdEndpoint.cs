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
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetAllByCustomerId([FromRoute] int customerId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllFavouritesByCustomerIdQuery(customerId, pageIndex, pageSize), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
