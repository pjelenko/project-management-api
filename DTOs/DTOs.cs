namespace ProjectManagement.DTOs
{
    public record LoginDto(string Username, string Password);
    public record RefreshDto(string RefreshToken);
}
