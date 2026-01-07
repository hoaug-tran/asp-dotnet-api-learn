using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnLinQWeb.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        [MaxLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string? Email { get; set; }
        [MaxLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string? AvatarUrl { get; set; }

        public User()
        {

        }

        public User(int id, string name, string username, string password, string? email, string? avatarUrl, string role)
        {
            Id = id;
            Name = name;
            Username = username;
            Password = password;
            Email = email;
            AvatarUrl = avatarUrl;
            Role = role;
        }
    }
}
