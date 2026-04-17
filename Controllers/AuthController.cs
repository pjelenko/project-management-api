using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DTOs;
using ProjectManagement.Models;
using ProjectManagement.Repositories;
using ProjectManagement.Services;

namespace ProjectManagement.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(PMDbContext db, TokenService tokenService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            if (db.Users.Any(u => u.Username == dto.Username))
                return BadRequest("User exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await db.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized();

            var token = tokenService.CreateToken(user);
            var refresh = tokenService.CreateRefreshToken();

            user.RefreshTokens.Add(refresh);
            await db.SaveChangesAsync();

            return Ok(new
            {
                token,
                refreshToken = refresh.Token
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshDto dto)
        {
            var token = await db.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == dto.RefreshToken);

            if (token == null || token.IsRevoked || token.Expires < DateTime.UtcNow)
                return Unauthorized();

            token.IsRevoked = true;

            var newRefresh = tokenService.CreateRefreshToken();
            token.User.RefreshTokens.Add(newRefresh);

            var newJwt = tokenService.CreateToken(token.User);

            await db.SaveChangesAsync();

            return Ok(new
            {
                token = newJwt,
                refreshToken = newRefresh.Token
            });
        }
    }
}
