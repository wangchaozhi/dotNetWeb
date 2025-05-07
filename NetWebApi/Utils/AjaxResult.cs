using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json.Serialization;

namespace NetWebApi.Utils
{
    // General response structure with timestamp and traceId
    public class ApiResponse<T>
    {
        public int Code { get; set; }           // Status code
        public string Message { get; set; }     // Response message
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // 当 Data 为 null 时忽略
        public T Data { get; set; }             // Generic data
        public long Timestamp { get; set; }     // Response timestamp
        public string TraceId { get; set; }     // Request trace ID

        // Constructor to initialize the response
     
        public ApiResponse(int code, string message, T data, string traceId)
        {
            Code = code;
            Message = message;
            Data = data;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            TraceId = string.IsNullOrEmpty(traceId) ? Guid.NewGuid().ToString() : traceId;
        }
    }

    public static class AjaxResult
    {
        // Success response
        public static IActionResult Success<T>(T data, string traceId = null)
        {
            var response = new ApiResponse<T>(200, "操作成功", data, traceId);
            return new OkObjectResult(response);
        }

        // Failure response
        public static IActionResult Fail(string message, int code = 400, string traceId = null)
        {
            var response = new ApiResponse<object>(code, message, null, traceId);
            return new OkObjectResult(response); // You can use StatusCode instead of OkObjectResult if you want to change the status code
        }
    }
}