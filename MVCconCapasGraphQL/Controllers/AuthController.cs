using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MVCconCapasGraphQL.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly string _cs;
        private readonly IConfiguration _cfg;

        public AuthController(IConfiguration cfg)
        {
            _cfg = cfg;
            _cs = cfg.GetConnectionString("Beerhenqe");
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; } 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req?.Email) || string.IsNullOrWhiteSpace(req?.Password))
                return BadRequest(new { code = -200, message = "Email y Password son requeridos" });

            using var conn = new SqlConnection(_cs);
            await conn.OpenAsync();

            var user = await conn.QueryFirstOrDefaultAsync(
                @"SELECT TOP 1 u.Id, u.Email, u.PasswordHash, u.DisplayName, u.IsActive
                  FROM sec.Users u
                  WHERE u.Email = @Email AND u.PasswordHash = @Password AND u.IsActive = 1",
                new { req.Email, req.Password });

            if (user == null)
                return Unauthorized(new { code = -200, message = "Credenciales inválidas" });

            var role = await conn.ExecuteScalarAsync<string>(
                @"SELECT TOP 1 r.Name
                  FROM sec.UserRoles ur
                  JOIN sec.Roles r ON r.Id = ur.RoleId
                  WHERE ur.UserId = @UserId
                  ORDER BY r.Priority DESC",
                new { UserId = (int)user.Id }) ?? "user";

            var secret = _cfg["Jwt:Secret"];
            var issuer = _cfg["Jwt:Issuer"];
            var audience = _cfg["Jwt:Audience"];
            var expiresHours = int.TryParse(_cfg["Jwt:ExpiresHours"], out var h) ? h : 4;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, (string)user.Email),
                new Claim(ClaimTypes.Name, (string)user.DisplayName),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expiresHours),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                code = 100,
                message = "OK",
                data = new
                {
                    token = jwt,
                    expires = token.ValidTo,
                    user = new
                    {
                        id = user.Id,
                        email = user.Email,
                        displayName = user.DisplayName,
                        role
                    }
                }
            });
        }
    }
}
