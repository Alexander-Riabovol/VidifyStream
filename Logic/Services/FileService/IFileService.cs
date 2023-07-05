using Data.Dtos;
using Microsoft.AspNetCore.Http;

namespace Logic.Services.FileService
{
    /// <summary>
    /// Represents a file service interface for handling file operations such as uploading and downloading.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Uploads a file to the file storage.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>A <see cref="ServiceResponse{T}"/> indicating the result of the upload operation,
        /// where T is a new assigned name of the uploaded file</returns>
        Task<ServiceResponse<string>> Upload(IFormFile file);
        /// <summary>
        /// Downloads a file from the file storage.
        /// </summary>
        /// <param name="fileName">The name of the file to download.</param>
        /// <returns>A <see cref="ServiceResponse{T}"/> indicating the result of the download operation,
        /// where T is a tuple containing the file data as a byte array and the content type of the file.</returns>
        Task<ServiceResponse<(byte[] Data, string ContentType)>> Download(string fileName);
    }
}