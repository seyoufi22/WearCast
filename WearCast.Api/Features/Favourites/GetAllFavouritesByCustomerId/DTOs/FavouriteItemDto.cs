namespace WearCast.Api.Features.Favourites.GetAllFavouritesByCustomerId.DTOs;

public class FavouriteItemDto
{
    public int FixedProductColorId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ColorName { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
}
