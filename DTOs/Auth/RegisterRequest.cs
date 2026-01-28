namespace LearnLinQWeb.DTOs.Auth;

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VerifyPassword { get; set; } = string.Empty;
}
