namespace LearnLinQWeb.DTOs.Users;

public class CreateUserRequest
{
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? VerifyPassword { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Role { get; set; }
}
