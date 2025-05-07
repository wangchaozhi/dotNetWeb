using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;


namespace NetWebApi.Controller;

    [ApiController]
    [Route("api/[controller]")]
    public class ImageUploadController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDto fileUploadDto)
        {
            if (fileUploadDto.File == null || fileUploadDto.Md5 == null)
            {
                return BadRequest("File or MD5 hash is missing.");
            }

            // 获取上传的文件
            var file = fileUploadDto.File;

            // 验证 MD5 值
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                var calculatedMd5 = CalculateMd5(fileBytes);

                if (calculatedMd5 != fileUploadDto.Md5)
                {
                    return BadRequest("MD5 hash does not match.");
                }

                // 保存文件
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, file.FileName);
                await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

                return Ok(new { Message = "File uploaded successfully!", FilePath = filePath });
            }
        }

        private string CalculateMd5(byte[] fileBytes)
        {
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(fileBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }

    public class FileUploadDto
    {
        public IFormFile File { get; set; }
        public string Md5 { get; set; }
    }

