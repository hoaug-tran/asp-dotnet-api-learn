namespace LearnLinQWeb.DTOs.Users;

public class UserResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Role { get; set; }
}
