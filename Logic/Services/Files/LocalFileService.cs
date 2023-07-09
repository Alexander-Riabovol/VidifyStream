using Data.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Http.Headers;
using System.Text;

namespace Logic.Services.Files
{
    /// <summary>
    /// Represents a file service implementation that handles local file storage.
    /// This class provides methods for uploading and downloading files,
    /// as well as generating unique file names for uploaded files.
    /// </summary>
    public class LocalFileService : IFileService
    {
        /// <summary>
        /// The local path where the files are stored.
        /// </summary>
        public static string LocalBlobStoragePath { get; }
        private static readonly string _rangeMediaType = "text/plain";

        private readonly IHttpContextAccessor _accessor;
        public LocalFileService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        static LocalFileService() 
        { 
            // Get Current Directory
            var sb = new StringBuilder(Directory.GetCurrentDirectory());
            // Retrive Parent Directory
            while (sb[sb.Length - 1] != Path.DirectorySeparatorChar)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("Files");
            // Create the 'Files' directory if does not exist
            if (!Directory.Exists(sb.ToString()))
            {
                Directory.CreateDirectory(sb.ToString());
            }
            sb.Append(Path.DirectorySeparatorChar);
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

            var scheme = _accessor.HttpContext!.Request.Scheme;
            var host = _accessor.HttpContext!.Request.Host.ToUriComponent();
            var filePath = $"{scheme}://{host}/api/download/{newFileName}";

            return ServiceResponse<string>.OK(filePath);
        }

        public async Task<ServiceResponse<(byte[], string)>> Download(string fileName)
        {
            if(!File.Exists(LocalBlobStoragePath + fileName))
            {
                return new ServiceResponse<(byte[], string)>(404, $"The File {fileName} does not exist");
            }
            await using var stream = File.OpenRead(LocalBlobStoragePath + fileName);

            var rangeHeader = _accessor.HttpContext!.Request.Headers.Range;
            if (rangeHeader.Any())
            {
                RangeHeaderValue rangeHeaderValue;
                bool parseResult = RangeHeaderValue.TryParse(rangeHeader, out rangeHeaderValue!);

                if(!parseResult) 
                {
                    return new ServiceResponse<(byte[], string)>(400, "The format of Range header is invalid.");
                }

                var range = new ByteRangeStreamContent(stream, rangeHeaderValue, _rangeMediaType);

                var rangeResult = await range.ReadAsByteArrayAsync();

                return ServiceResponse<(byte[], string)>.OK((rangeResult, _rangeMediaType));
            }

            byte[] result = new byte[stream.Length];
            
            await stream.ReadAsync(result, 0, result.Length);

            var provider = new FileExtensionContentTypeProvider();
            if(!provider.TryGetContentType(stream.Name, out var contentType))
            {
                contentType = "application/octec-stream";
            }

            return ServiceResponse<(byte[], string)>.OK((result, contentType));
        }
        // I don't like Path.GetRandomFileName() method, so I wrote mine.
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
