namespace WearCast.Api.Common.Validators
{
    public static class FileValidatorExtensions
    {
        private static readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private const int MaxFileSizeInBytes = 2 * 1024 * 1024;

        public static IRuleBuilderOptions<T, IFormFile> IsValidImage<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
        {
            return ruleBuilder
                .Must(file =>
                {
                    if (file is null) return true;

                    return file.Length > 0 && file.Length <= MaxFileSizeInBytes;
                })
                .WithMessage($"File must be between 0 and {MaxFileSizeInBytes / 1024 / 1024} MB.")

                .Must(file =>
                {
                    if (file is null) return true;

                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    return _allowedExtensions.Contains(ext);
                })
                .WithMessage($"Invalid file type. Allowed: {string.Join(", ", _allowedExtensions)}");
        }
    }
}
