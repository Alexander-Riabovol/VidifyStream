using System.Text.Json.Serialization;

namespace Data.Models
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum NotificationType
    {
        SubscribersGoal = 0,
        Reply = 1,
        AuthorLikedComment = 2,
        LeftComment = 3,
        NewSubscribtionsVideo = 4,
        RecommendedVideo = 5,
    }
}