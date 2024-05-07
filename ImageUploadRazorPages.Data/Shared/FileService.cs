using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ImageUploadRazorPages.Data.Shared;

public interface IFileService
{
    void DeleteFile(string fileName, string directory);
    Task<string> SaveFile(IFormFile file, string directory, string[] allowedExtensions);
}

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> SaveFile(IFormFile file, string directory, string[] allowedExtensions)
    {
        // path of the `wwwroot` folder
        var wwwPath = _webHostEnvironment.WebRootPath;
        var path = Path.Combine(wwwPath, directory);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        var extension = Path.GetExtension(file.FileName);
        if (!allowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException($"Only {string.Join(",", allowedExtensions)} are allowed");
        }
        var newFileName = $"{Guid.NewGuid()}{extension}";
        var fileWithPath = Path.Combine(path, newFileName);
        using var fileStream = new FileStream(fileWithPath, FileMode.Create);
        await file.CopyToAsync(fileStream);
        return newFileName;
    }

    public void DeleteFile(string fileName, string directory)
    {
        var fileWithPath = Path.Join(_webHostEnvironment.WebRootPath, directory, fileName);
        if (!Path.Exists(fileWithPath))
        {
            throw new FileNotFoundException($"{fileName} does not exists");
        }
        File.Delete(fileWithPath);
    }

}
