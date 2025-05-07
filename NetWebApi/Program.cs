using System.Reflection;
using Microsoft.OpenApi.Models;
using NetWebApi.Config;
using NetWebApi.Services;
using NetWebApi.Utils;

var builder = WebApplication.CreateBuilder(args);
// 添加 CORS 策略服务
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // 允许所有来源（开发阶段）
            .AllowAnyMethod()   // 允许所有 HTTP 方法
            .AllowAnyHeader();  // 允许所有请求头
    });
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// 配置 SwaggerGen，启用注解支持
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations(); // 启用注解支持
    
    // 默认文档 v1
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "默认 API 文档", 
        Version = "v1" 
    });
    // 自定义分组 CustomGroup
    c.SwaggerDoc("CustomGroup", new OpenApiInfo 
    { 
        Title = "自定义 API 文档", 
        Version = "v1" 
    });

   
});
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddControllers();  // 注册 MVC 控制器服务
// 注册服务
builder.Services.AddAuthentication(); // 注册身份验证服务
builder.Services.AddAuthorization();  // 注册授权服务

// builder.Services.AddScoped<ApplicationDbContext>();
// builder.Services.AddScoped<UserService>();
// // Add services to the container.
// builder.Services.AddScoped<JWTUtils>();  // 注册JWTUtils为作用域服务


// 自动扫描和注册服务
builder.Services.AddServicesFromAttributes(Assembly.GetExecutingAssembly());


// 添加路由配置，使用自定义的小驼峰转换器
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true; // 可选：确保生成的小驼峰路由是小写
});


var app = builder.Build();

// 必须在 UseRouting 之前调用 UseCors
app.UseCors("AllowAll");

// 使用封装的 JWT 认证服务配置


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/CustomGroup/swagger.json", "自定义 API 文档");
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "默认 API 文档 v1");
    });
}

// app.UseHttpsRedirection(); // 你可以注释掉或删除这行代码以禁用 HTTPS 重定向 
// 使用中间件
app.UseAuthentication();  // 先验证用户身份
app.UseAuthorization();   // 然后检查用户是否有访问权限

// 设置所有控制器的基础起始路径
app.MapControllers();
// app.UseHttpsRedirection(); 


app.Run();
