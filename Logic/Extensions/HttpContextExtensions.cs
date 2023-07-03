using Data.Dtos;
using Microsoft.AspNetCore.Http;

namespace Logic.Extensions
{
    public static class HttpContextExtensions
    {
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
