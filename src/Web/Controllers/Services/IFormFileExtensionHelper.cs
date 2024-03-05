using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using DMS.Domain.Dto.StreamDto;

namespace DMS.Web.Controllers.Services
{
    public static class IFormFileExtensionHelper
    {
        public static IFormFile ToIFormFile(this Stream stream, string fileName)
        {
            return new StreamFormFile(stream, stream.Length, "application/octet-stream", "file", fileName);
        }
        public static string ToBase64String(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
