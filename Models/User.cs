namespace LearnLinQWeb.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User()
        {

        }

        public User(int id, string name, string username, string password, string role)
        {
            Id = id;
            Name = name;
            Username = username;
            Password = password;
            Role = role;
        }

    }
}
