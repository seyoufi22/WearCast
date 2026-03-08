using WearCast.Api.Abstractions;

namespace WearCast.Api.Features.FixedProduct.Errors;

public static class FixedProductErrors
{
    public static readonly Error DuplicateName = new(
        "FixedProduct.DuplicateName",
        "A product with the same name already exists.",
        StatusCodes.Status409Conflict);

    public static readonly Error CategoryNotFound = new(
        "FixedProduct.CategoryNotFound",
        "The specified Category was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error UserNotFound = new(
        "FixedProduct.UserNotFound",
        "The specified User (CreatedById) was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error ProductNotFound = new(
        "FixedProduct.ProductNotFound",
        "The specified Product was not found.",
        StatusCodes.Status404NotFound);
}
