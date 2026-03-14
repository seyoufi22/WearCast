namespace WearCast.Api.Features.CartManagment.AddOrUpdateCartItem;

using MediatR;
using Microsoft.EntityFrameworkCore;
using WearCast.Api.Entities;
using WearCast.Api.Features.CartManagment.AddOrUpdateCartItem.DTOs;


public class AddOrUpdateCartItemHandler(ApplicationDbContext context)
    : IRequestHandler<AddOrUpdateCartItemCommand, bool>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Handle(AddOrUpdateCartItemCommand command, CancellationToken cancellationToken)
    {
        var cartItem = await _context.CartItems
            .Include(c => c.Sizes)
            .FirstOrDefaultAsync(c =>
                c.ColorId == command.Request.ColorId &&
                c.CustomerId == command.CustomerId,
                cancellationToken);

        if (cartItem == null)
        {
            if (command.Request.Quantity <= 0)
                return true;

            cartItem = new CartItem
            {
                CustomerId = command.CustomerId,
                ColorId = command.Request.ColorId
            };

            cartItem.AddOrUpdateSize(command.Request.Size, command.Request.Quantity);

            await _context.CartItems.AddAsync(cartItem, cancellationToken);
        }
        else 
        {
            cartItem.AddOrUpdateSize(command.Request.Size, command.Request.Quantity);

            if (!cartItem.Sizes.Any())
            {
                _context.CartItems.Remove(cartItem);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}