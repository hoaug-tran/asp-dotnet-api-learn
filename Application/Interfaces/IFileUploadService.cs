namespace LearnLinQWeb.Application.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadAvatarAsync(IFormFile file, int userId);
    Task<bool> DeleteAvatarAsync(string filePath);
    bool IsValidImage(IFormFile file);
}
