using WearCast.Api.Abstractions;

namespace WearCast.Api.Features.FixedProduct.Errors;

public static class FixedProductErrors
{
    public static Error DuplicateName(string productName) => new(
        "FixedProduct.DuplicateName",
        $"A product with the name '{productName}' already exists. Please choose a different name.",
        StatusCodes.Status409Conflict);

    public static Error CategoryNotFound(int categoryId) => new(
        "FixedProduct.CategoryNotFound",
        $"Category with ID {categoryId} was not found. Please provide a valid CategoryId.",
        StatusCodes.Status404NotFound);

    public static Error UserNotFound(string userId) => new(
        "FixedProduct.UserNotFound",
        $"User with ID '{userId}' was not found. Please ensure the user exists.",
        StatusCodes.Status404NotFound);

    public static readonly Error ProductNotFound = new(
        "FixedProduct.ProductNotFound",
        "The specified Product was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error SizeDetailsEmpty = new(
        "FixedProduct.SizeDetailsEmpty",
        "At least one size detail is required. Please add size measurements (A, B, C) for at least one size.",
        StatusCodes.Status400BadRequest);
}
