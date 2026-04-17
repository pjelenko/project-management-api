namespace ProjectManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "User";

        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
