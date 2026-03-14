using WearCast.Api.Features.CartManagment.DeleteCartItem.DTOs;

namespace WearCast.Api.Features.CartManagment.DeleteCartItem;

public class DeleteCartItemHandler(ApplicationDbContext context)
    : IRequestHandler<DeleteCartItemCommand, bool>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Handle(DeleteCartItemCommand command, CancellationToken cancellationToken)
    {
        await _context.CartItems
            .Where(c => c.CustomerId == command.CustomerId && c.ColorId == command.ColorId)
            .ExecuteDeleteAsync(cancellationToken);

        return true;
    }
}