using VidifyStream.Data.Dtos;
using Microsoft.AspNetCore.Http;

namespace VidifyStream.Logic.Extensions
{
    /// <summary>
    /// Extension methods for HttpContext related operations.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Retrieves the user ID from the HttpContext.
        /// </summary>
        /// <returns>A <see cref="ServiceResponse"/> containing the user ID if successful, or an error response.</returns>
        public static ServiceResponse<int> RetriveUserId(this HttpContext context)
        {
            int id;
            var strId = context.User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (!int.TryParse(strId, out id))
            {
                return new ServiceResponse<int>(401, "Unauthorized");
            }
            return ServiceResponse<int>.OK(id);
        }
    }
}
