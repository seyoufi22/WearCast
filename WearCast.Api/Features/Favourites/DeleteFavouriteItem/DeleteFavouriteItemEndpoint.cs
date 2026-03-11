using WearCast.Api.Features.Favourites.DeleteFavouriteItem.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WearCast.Api.Features.Favourites.DeleteFavouriteItem.DTOs;

namespace WearCast.Api.Features.Favourites.DeleteFavouriteItem;

[ApiController]
[Route("api/Favourites/Delete")]
[Tags("Favourites")]
public class DeleteFavouriteItemEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public DeleteFavouriteItemEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteFavouriteItem([FromBody] DeleteFavouriteItemRequestDto request, CancellationToken cancellationToken = default)
    {
        var customerId = HttpContext.User.GetCustomerId();
        if (customerId == null)
            return Unauthorized("User is not a valid customer.");

        var result = await _sender.Send(new DeleteFavouriteItemCommand(customerId.Value, request.FixedProductColorId), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
