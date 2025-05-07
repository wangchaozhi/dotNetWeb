using Microsoft.AspNetCore.Authorization;
using NetWebApi.Models;
using NetWebApi.Utils;

namespace NetWebApi.Controller;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    private readonly JWTUtils _jwtUtils;

    public WeatherForecastController(JWTUtils jwtUtils)
    {
        _jwtUtils = jwtUtils;
    }

    [HttpGet("GetJWT")]
    public ActionResult<string> GetJWT()
    {
        var user = new User();
        user.Username = "wang";
        user.Id = 1;
        string token = _jwtUtils.GenerateJwtToken(user);
        return Ok(token);  // 返回生成的 JWT
    }
    
    [Authorize]
    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            Summaries[Random.Shared.Next(Summaries.Length)]
        )).ToArray();
    }


    [HttpPost]
    public IActionResult Post([FromBody] WeatherForecast forecast)
    {
        // 实际应用中，你应该将 forecast 保存到数据库
        // 这里我们仅返回201 Created状态码
        return CreatedAtAction(nameof(Get), new { date = forecast.Date }, forecast);
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
