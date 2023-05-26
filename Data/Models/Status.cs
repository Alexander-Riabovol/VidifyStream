using System.Text.Json.Serialization;

namespace Data.Models
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Status
    {
        Unverified = 0,
        User = 1,
        Janitor = 2,
        Admin = 3,
        Banned = 4,
    }
}
