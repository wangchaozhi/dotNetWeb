using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using NetWebApi.Models;
using NetWebApi.Services;
using NetWebApi.Utils;
using Newtonsoft.Json;

namespace NetWebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly JWTUtils _jwtUtils;

    public UserController(UserService userService, JWTUtils jwtHelper)
    {
        _userService = userService;
        _jwtUtils = jwtHelper;
    }

    // 登录
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _userService.Authenticate(request.Username, request.Password);
        if (user != null)
        {
            var token = _jwtUtils.GenerateJwtToken(user);
            return AjaxResult.Success(new { Token = token });
        }

        return AjaxResult.Fail("用户名或密码错误");
    }
    
    
    [Authorize]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        
        // 打印 HttpContext 信息
        var context = HttpContext;
    
        // // 打印请求方法和路径
        // Console.WriteLine($"Request Method: {context.Request.Method}");
        // Console.WriteLine($"Request Path: {context.Request.Path}");
        // 获取JWT中的用户名（sub声明）
        
        // 打印所有声明
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }
        // 使用 NameIdentifier 提取用户 ID 或用户名
        var username = User.Claims.FirstOrDefault(c => c.Type ==ClaimTypes.Name)?.Value;
        // 使用 NameIdentifier 提取用户 ID
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        // var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        
    
        if (username == null)
        {
            return Unauthorized("无法从令牌中提取用户名");
        }
    
        return AjaxResult.Success(new { Username = username });
    }
    
    
    
    // [Authorize]
    // [HttpGet("profile")]
    // public IActionResult GetProfile()
    // {
    //     // 从 Claim 中获取序列化的用户数据
    //     var userJson = User.Claims.FirstOrDefault(c => c.Type == "user_data")?.Value;
    //
    //     if (userJson == null)
    //     {
    //         return Unauthorized("无法从令牌中提取用户信息");
    //     }
    //
    //     // 将 JSON 反序列化为 User 对象
    //     var user = JsonConvert.DeserializeObject<User>(userJson);
    //
    //     return Ok(new { user.Username, user.Email, user.Role });
    // }


    [Authorize]
    [HttpGet("jwt-id")]
    public IActionResult GetJwtId()
    {
        // 获取JWT中的ID（jti声明）
        var jwtId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

        if (jwtId == null)
        {
            return Unauthorized("无法从令牌中提取JWT ID");
        }

        return AjaxResult.Success(new { JwtId = jwtId });
    }

    // 注册
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (_userService.UserExists(request.Username))
        {
            return  AjaxResult.Fail("用户名已存在");
        }

        var newUser = new User
        {
            Username = request.Username,
            Password = request.Password
        };
        _userService.Register(newUser);
        return AjaxResult.Success(newUser);
    }

    // 获取用户列表（必须登录后才能访问）
    [Authorize]
    [HttpGet("list")]
    public IActionResult GetUserList()
    {
        var users = _userService.GetUsers();
        return AjaxResult.Success(users);
    }

    // 删除用户（必须登录后才能访问）
    [Authorize]
    [HttpDelete("delete/{id}")]
    public IActionResult DeleteUser(int id)
    {
        _userService.DeleteUser(id);
        return AjaxResult.Success($"用户 {id} 删除成功");
    }
}