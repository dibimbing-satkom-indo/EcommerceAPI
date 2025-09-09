using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult GetWelcomeMsg()
    {
        var response = "Hello world";
        return Ok(response);

    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            Status = "Healthy",
            Serice = "E-Commerce API",
            Uptime = DateTime.Now
        });
    }


}