namespace WearCast.Api.Persistence.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !allowedExtensions.Contains(ext))
                return $"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}";
            var rootPath = _environment.WebRootPath;
            if (string.IsNullOrEmpty(rootPath))
            {
                throw new DirectoryNotFoundException("wwwroot folder is missing in the API project.");
            }

            var uploadsFolder = Path.Combine(rootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}{ext}";

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"https://localhost:7250/uploads/{uniqueFileName}";
        }
        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                throw new ArgumentException("File URL is required.");

            var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);

            var rootPath = _environment.WebRootPath;
            if (string.IsNullOrEmpty(rootPath))
            {
                throw new DirectoryNotFoundException("wwwroot folder is missing in the API project.");
            }

            var filePath = Path.Combine(rootPath, "uploads", fileName);

            if (!File.Exists(filePath))
            {
                return false; 
            }

            await Task.Run(() => File.Delete(filePath));

            return true; 
        }
    }
}
