using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.DTOs.Auth;

public class RegisterResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
}
