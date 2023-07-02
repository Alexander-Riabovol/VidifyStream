using Data.Dtos;
using Microsoft.AspNetCore.Http;

namespace Logic.Extensions
{
    public static class HttpContextExtensions
    {
        public static ServiceResponse<int> RetriveUserId(this HttpContext context)
        {
            int id;
            if (!int.TryParse(context.User!.Claims.First(c => c.Type == "id")!.Value, out id))
            {
                return new ServiceResponse<int>(401, "Unauthorized");
            }
            return ServiceResponse<int>.OK(id);
        }
    }
}
