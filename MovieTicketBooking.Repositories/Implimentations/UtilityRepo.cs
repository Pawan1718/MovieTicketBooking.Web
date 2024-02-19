using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MovieTicketBooking.Repositories.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repositories.Implimentations
{
    public class UtilityRepo : IUtilityRepo
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UtilityRepo(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> DeleteImage(string ContainerName, string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                return null;
            }

            var fileName = Path.GetFileName(dbPath);
            var completePath = Path.Combine(_environment.WebRootPath, ContainerName, fileName);
            if (File.Exists(completePath))
            {
                File.Delete(completePath);
            }

            return null;
        }

        public async Task<string> EditImage(string ContainerName, IFormFile file, string dbPath)
        {
            await DeleteImage(ContainerName, dbPath);
            return await SaveImage(ContainerName, file);
        }

        public async Task<string> SaveImage(string ContainerName, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_environment.WebRootPath, ContainerName);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, fileName);
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(fileStream);
                }
            }
            var basePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var completePath = Path.Combine(basePath, ContainerName, fileName).Replace("\\", "/");
            return completePath;
        }
    }
}
