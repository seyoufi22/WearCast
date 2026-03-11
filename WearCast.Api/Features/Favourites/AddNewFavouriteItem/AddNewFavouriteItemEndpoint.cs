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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddNewFavouriteItem([FromBody] AddNewFavouriteItemRequestDto request, CancellationToken cancellationToken = default)
    {
        // Typically, you might extract CustomerId from the User claims, 
        // but since the task specifies using the request values:
        var result = await _sender.Send(new AddNewFavouriteItemCommand(request.CustomerId, request.FixedProductColorId), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
