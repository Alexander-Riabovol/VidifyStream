using Data.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.Text;

namespace Logic.Services.FileService
{
    public class LocalFileService : IFileService
    {
        public static string LocalBlobStoragePath { get; }

        static LocalFileService() 
        { 
            // Get Current Directory
            var sb = new StringBuilder(Directory.GetCurrentDirectory());
            // Retrive Parent Directory
            while (sb[sb.Length - 1] != '\\')
            {
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("Files");
            // Create the 'Files' directory if does not exist
            if (!Directory.Exists(sb.ToString()))
            {
                Directory.CreateDirectory(sb.ToString());
            }
            sb.Append('\\');
            LocalBlobStoragePath = sb.ToString();
        }

        public async Task<ServiceResponse<string>> Upload(IFormFile file)
        {
            var fileNameParts = file.FileName.Split('.');
            // if file has no extension, set it to empty string, otherwise to whatever the extension is.
            var extension = fileNameParts.Length == 1 ? "" : '.' + fileNameParts.Last();
            // make up a new name for the file
            var newFileName = GenerateFileName(extension);
            while (File.Exists(LocalBlobStoragePath + newFileName))
            {
                newFileName = GenerateFileName(extension);
            }

            await using var stream = File.OpenWrite(LocalBlobStoragePath + newFileName);
            await file.CopyToAsync(stream);

            return ServiceResponse<string>.OK(newFileName);
        }

        public async Task<ServiceResponse<(byte[], string)>> Download(string fileName)
        {
            if(!File.Exists(LocalBlobStoragePath + fileName))
            {
                return new ServiceResponse<(byte[], string)>(404, $"The File {fileName} does not exist");
            }
            await using var stream = File.OpenRead(LocalBlobStoragePath + fileName);
            byte[] result = new byte[stream.Length];
            
            await stream.ReadAsync(result, 0, result.Length);

            var provider = new FileExtensionContentTypeProvider();
            if(!provider.TryGetContentType(stream.Name, out var contentType))
            {
                contentType = "application/octec-stream";
            }

            return ServiceResponse<(byte[], string)>.OK((result, contentType));
        }

        private string GenerateFileName(string extension)
        {
            var random = new Random();
            var sb = new StringBuilder();
            int count = random.Next(7, 12);
            for (; count > 0; count--)
            {
                sb.Append((char)random.Next('A', 'Z' + 1));
            }
            sb.Append(extension);
            return sb.ToString();
        }
    }
}
