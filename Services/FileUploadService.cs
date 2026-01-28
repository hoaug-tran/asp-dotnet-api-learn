using LearnLinQWeb.Application.Interfaces;

namespace LearnLinQWeb.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public FileUploadService(IConfiguration config, IWebHostEnvironment env)
    {
        _config = config;
        _env = env;
    }

    public async Task<string> UploadAvatarAsync(IFormFile file, int userId)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File rỗng");
        }

        if (!IsValidImage(file))
        {
            throw new ArgumentException("Loại file không hợp lệ. Chỉ có jpg, jpeg, png, gif là được cho phép");
        }
        try
        {
            var uploadFolder = _config["FileUpload:AvatarFolder"];
            var maxFileSize = long.Parse(_config["FileUpload:MaxFileSize"] ?? "5242880");

            if (file.Length > maxFileSize)
            {
                throw new ArgumentException($"Kích thước tệp vượt quá giới hạn tối đa {maxFileSize} bytes");
            }
            var uploadPath = Path.Combine(_env.ContentRootPath, uploadFolder);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{userId}_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var baseUrl = _config["FileUpload:BaseUrl"];
            var relativePath = $"{uploadFolder.Replace("\\", "/")}".Replace("\\", "/");
            var avatarUrl = $"{baseUrl}/{relativePath}/{fileName}";

            return avatarUrl;
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi upload file: {ex.Message}");
        }
    }

    public async Task<bool> DeleteAvatarAsync(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return true;
            }

            var fileName = Path.GetFileName(filePath);
            var uploadFolder = _config["FileUpload:AvatarFolder"];
            var fullPath = Path.Combine(_env.ContentRootPath, uploadFolder, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting file: {ex.Message}");
        }
    }

    public bool IsValidImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return false;
        }
            

        var allowedExtensions = _config.GetSection("FileUpload:AllowedExtensions").Get<List<string>>();
        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(fileExtension))
        {
            return false;
        }
            

        var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };
        if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
        {
            return false;
        }
            

        return true;
    }
}
