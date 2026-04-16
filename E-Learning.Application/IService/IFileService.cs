using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Application.IService
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderPath);
        Task<bool> DeleteFileAsync(string filePath);
        bool IsValidFileType(IFormFile file, string[] allowedExtensions);
        long GetFileSizeInMegabytes(IFormFile file);
    }
}
