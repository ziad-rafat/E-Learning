using E_Learning.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace E_Learning.Application.Services
{
    public class FileService : IFileService
    {
        private readonly string _rootPath;
        private const long MaxFileSizeInMB = 100;
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly string[] _allowedVideoExtensions = { ".mp4", ".mov", ".wmv", ".avi" };

        public FileService(IConfiguration configuration)
        {
            _rootPath = configuration["FileStorage:RootPath"]
                ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            EnsureRootFolderExists();
        }

        private void EnsureRootFolderExists()
        {
            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }
        }

        private void EnsureDirectoryExists(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);

                    // Set appropriate directory permissions if needed
                    // var directoryInfo = new DirectoryInfo(directoryPath);
                    // directoryInfo.Attributes &= ~FileAttributes.ReadOnly;
                }
            }
            catch (Exception ex)
            {
                throw new DirectoryNotFoundException(
                    $"Failed to create or access directory: {directoryPath}. Error: {ex.Message}");
            }
        }

        private string NormalizePath(string folderPath)
        {
            // Remove any leading or trailing slashes and replace backward slashes
            folderPath = folderPath.Trim('/').Trim('\\').Replace('\\', '/');

            // Ensure the path doesn't try to navigate up directories
            if (folderPath.Contains(".."))
                throw new ArgumentException("Invalid folder path");

            return folderPath;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("Invalid file");

                if (GetFileSizeInMegabytes(file) > MaxFileSizeInMB)
                    throw new ArgumentException($"File size exceeds maximum limit of {MaxFileSizeInMB}MB");

                // Validate file type
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                var isImage = _allowedImageExtensions.Contains(fileExtension);
                var isVideo = _allowedVideoExtensions.Contains(fileExtension);

                if (!isImage && !isVideo)
                    throw new ArgumentException("Invalid file type");

                // Normalize and validate the folder path
                folderPath = NormalizePath(folderPath);

                // Generate unique file name
                var fileName = $"{Guid.NewGuid()}{fileExtension}";

                // Combine paths and create full directory path
                var directoryPath = Path.Combine(_rootPath, folderPath);
                var filePath = Path.Combine(directoryPath, fileName);

                // Ensure directory exists
                EnsureDirectoryExists(directoryPath);

                // Create file stream and copy file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return relative path for storage in database
                return Path.Combine(folderPath, fileName).Replace('\\', '/');
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload file: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            try
            {
                filePath = NormalizePath(filePath);
                var fullPath = Path.Combine(_rootPath, filePath);

                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));

                    // Optionally remove empty directories
                    await CleanEmptyDirectoriesAsync(Path.GetDirectoryName(fullPath));

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete file: {ex.Message}", ex);
            }
        }

        private async Task CleanEmptyDirectoriesAsync(string directory)
        {
            try
            {
                if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
                    return;

                // Don't delete the root upload directory
                if (directory.Equals(_rootPath, StringComparison.OrdinalIgnoreCase))
                    return;

                await Task.Run(() =>
                {
                    if (!Directory.EnumerateFileSystemEntries(directory).Any())
                    {
                        Directory.Delete(directory);
                        // Recursively clean parent directories
                        CleanEmptyDirectoriesAsync(Path.GetDirectoryName(directory)).Wait();
                    }
                });
            }
            catch
            {
                // Ignore errors in cleanup
            }
        }

        public bool IsValidFileType(IFormFile file, string[] allowedExtensions)
        {
            if (file == null || file.Length == 0)
                return false;

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }

        public long GetFileSizeInMegabytes(IFormFile file)
        {
            return file.Length / 1024 / 1024;
        }
    }
}
