using Microsoft.AspNetCore.Mvc;
using NetWebApi.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace NetWebApi.Controller;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "CustomGroup")]  // 自定义分组名称
[SwaggerTag("示例API")]  // 添加自定义标签
public class SampleController : ControllerBase
{
    [SwaggerOperation(Summary = "获取用户的详细资料", Description = "根据用户的ID返回详细资料")]
    [SwaggerResponse(200, "返回用户的详细资料")]
    [SwaggerResponse(404, "找不到用户")]
    [HttpGet("success")]
    public IActionResult GetSuccessResponse()
    {
        var data = new { Name = "Test", Value = 123 };
        return AjaxResult.Success(data);
    }

    [HttpGet("fail")]
    public IActionResult GetFailureResponse()
    {
        return AjaxResult.Fail("Something went wrong", 500);
    }
}
