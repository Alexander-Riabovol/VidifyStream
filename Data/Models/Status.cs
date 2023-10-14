using System.Text.Json.Serialization;

namespace VidifyStream.Data.Models
{
    /// <summary>
    /// Represents a status of a user, which dictates restrictions
    /// and rights to use certain api requests for logged customers.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Status
    {
        Unverified = 0,
        User = 1,
        Janitor = 2,
        Admin = 3,
        Banned = 4,
        SelfDeleted = 5,
    }
}
