using WearCast.Api.Features.Favourites.AddNewFavouriteItem.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WearCast.Api.Features.Favourites.AddNewFavouriteItem.DTOs;

namespace WearCast.Api.Features.Favourites.AddNewFavouriteItem;

[ApiController]
[Route("api/Favourites/Add")]
[Tags("Favourites")]
public class AddNewFavouriteItemEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public AddNewFavouriteItemEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = DefaultRoles.Customer)]
    [HttpPost]
    public async Task<IActionResult> AddNewFavouriteItem([FromBody] AddNewFavouriteItemRequestDto request, CancellationToken cancellationToken = default)
    {
        var customerId = HttpContext.User.GetCustomerId();
        if (customerId == null)
            return Unauthorized("User is not a valid customer.");

        var result = await _sender.Send(new AddNewFavouriteItemCommand(customerId.Value, request.FixedProductColorId), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
