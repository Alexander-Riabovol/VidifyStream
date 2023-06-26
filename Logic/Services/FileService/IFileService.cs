using Data.Dtos;
using Microsoft.AspNetCore.Http;

namespace Logic.Services.FileService
{
    public interface IFileService
    {
        Task<ServiceResponse<string>> Upload(IFormFile file);
    }
}