namespace WearCast.Api.Common.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };

        public (bool IsValid, string ErrorMessage) Validate(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return (false, "No file uploaded.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !_allowedExtensions.Contains(ext))
                return (false, $"Invalid file type. Allowed types: {string.Join(", ", _allowedExtensions)}");

            return (true, string.Empty);
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            var rootPath = _environment.WebRootPath;
            if (string.IsNullOrEmpty(rootPath))
                throw new DirectoryNotFoundException("wwwroot folder is missing.");

            var uploadsFolder = Path.Combine(rootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"https://localhost:7250/uploads/{uniqueFileName}";
        }
        public async Task<bool> DeleteAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                throw new ArgumentException("File URL is required.");

            var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
            var rootPath = _environment.WebRootPath;

            if (string.IsNullOrEmpty(rootPath))
                throw new DirectoryNotFoundException("wwwroot folder is missing.");

            var filePath = Path.Combine(rootPath, "uploads", fileName);

            if (!File.Exists(filePath)) return false;

            await Task.Run(() => File.Delete(filePath));
            return true;
        }
    }
}
