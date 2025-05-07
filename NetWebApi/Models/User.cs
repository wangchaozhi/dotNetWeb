namespace NetWebApi.Models;


    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        // 允许 Email 和 Role 为 null
        public string? Email { get; set; }
        public string? Role { get; set; }
    }

