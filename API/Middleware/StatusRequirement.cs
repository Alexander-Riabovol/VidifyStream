using VidifyStream.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace VidifyStream.API.Middleware
{
    /// <summary>
    /// Represents a requirement for the authorization based on User.Status property.
    /// </summary>
    public class StatusRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// The <see cref="Status"/> enum variables which are provided 
        /// into this requirement are the ones allowed to access an endpoint.
        /// </summary>
        public List<Status> AllowedStatuses { get; set; }
        public StatusRequirement() 
        {
            AllowedStatuses = new List<Status>() { Status.Admin, Status.Janitor, Status.User, Status.Unverified };
        }
        public StatusRequirement(params Status[] allowedStatuses) 
        { 
            AllowedStatuses = allowedStatuses.ToList();
        }
    }
}
