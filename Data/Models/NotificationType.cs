namespace VidifyStream.Data.Models
{
    /// <summary>
    /// Defines the type of a <see cref="Notification"/>.
    /// This enum is used internally for logic and is not directly accessible to a <see cref="User"/>.
    /// </summary>
    public enum NotificationType
    {
        Blank = -1,
        SubscribersGoal = 0,
        Reply = 1,
        AuthorLikedComment = 2,
        LeftComment = 3,
        NewSubscribtionsVideo = 4,
        RecommendedVideo = 5,
    }
}