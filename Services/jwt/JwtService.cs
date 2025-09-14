using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    public JwtService(IConfiguration config)
    {
        _config = config;

    }

    public string GenerateToke(User userDt)
    {
        //baca setting dari appsetting.json
        var secreet = _config["JwtSettings:Secret"];
        var issuer = _config["JwtSettings:Issuer"];
        var audience = _config["JwtSettings:Audience"];
        var exoiredMinutes = int.Parse(_config["JwtSettings:ExpiresMinutes"] ?? "60");

        //buat cliam yang akan di simpan di payload
        var claims = new List<Claim>
        {
            new Claim("UserID", userDt.UserID.ToString()),
            new Claim(ClaimTypes.Name, userDt.UserName),
            new Claim(ClaimTypes.Role, userDt.Role),
        };

        //buat key & signng cridential
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secreet!));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //buat token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(exoiredMinutes),
            signingCredentials: cred

        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}