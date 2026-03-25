namespace WearCast.Api.Features.FixedProductColor.Errors;

public class FixedProductColorErrors
{
    public static readonly Error ColorNotFound = new("Color.NotFound", "Product color not found.", 404);
    public static readonly Error UploadFailed = new("Image.UploadFailed", "An error occurred while uploading the image. Please try again.", 400);
    public static readonly Error SizeAlreadyExists = new("Size.AlreadyExists", "This size already exists for the selected color. Please update the quantity instead.", 400);
    public static Error ProductNotFound(int id) => new("Product.NotFound", $"Product with ID {id} was not found.", 404);
    public static readonly Error DuplicateHexCode = new("Color.DuplicateHexCode", "A color with this Hex Code already exists for this product.", 400);
    public static readonly Error InsufficientStock = new("Stock.Insufficient","The adjustment would result in a negative stock level, which is not allowed.",400);
    public static readonly Error ImageNotFound = new("Image.NotFound", "The requested image was not found.", 404);
    public static readonly Error ColorAlreadyExists = new("Color.AlreadyExists","A color with this Hex Code already exists for this product.",400);
}
