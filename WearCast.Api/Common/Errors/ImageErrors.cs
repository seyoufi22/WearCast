namespace WearCast.Api.Common.Errors
{
    public static class ImageErrors
    {
        public static readonly Error UploadFailed = new(
            "Image.UploadFailed",
            "Failed to save the image to the server disk.",
            StatusCodes.Status500InternalServerError);

        public static readonly Error UpdateFailed = new("Image.UpdateError",
            "An unexpected error occurred.",
           StatusCodes.Status500InternalServerError);

    }
}
