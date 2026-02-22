namespace WearCast.Api.Common.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };

        public ImageService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

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
            var webRootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadsFolder = Path.Combine(webRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            return $"{baseUrl}/uploads/{uniqueFileName}";
        }

        public async Task<bool> DeleteAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                throw new ArgumentException("File URL is required.");

            var webRootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");

            var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
            var filePath = Path.Combine(webRootPath, "uploads", fileName);

            if (!File.Exists(filePath)) return false;

            await Task.Run(() => File.Delete(filePath));
            return true;
        }
    }
}