using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _auth.RegisterAsync(dto);
        return StatusCode(201, result);

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _auth.LoginAsync(dto);
        return StatusCode(200, result);

    }

    [Authorize(Roles = "customer")]
    [HttpPost("complete-profile")]
    public async Task<IActionResult> CompleteProfileAsync([FromBody] CompleteProfileDto dto)
    {
        var userID = int.Parse(User.FindFirst("UserID")!.Value);
        var result = await _auth.CompleteProfileAsyync(userID, dto);
        return Ok(result);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("only admin can be access");
    }


}