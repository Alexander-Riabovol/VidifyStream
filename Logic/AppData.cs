namespace Logic
{
    // AppData Will be used as Singletone via Dependency Injector.
    /// <summary>
    /// Represents the application data.
    /// </summary>
    public class AppData
    {
        /// <summary>
        /// Represents a set of live signalr connections to the <see cref="NotificationsHub"/> 
        /// where Key is the id of the <see cref="Data.Models.User"/> and Value is the quantity of connections.
        /// </summary>
        public Dictionary<string, int> ActiveNotificationUsers { get; } = new Dictionary<string, int>();
        /// <summary>
        /// Gets a value indicating whether the application is running in a Docker container.
        /// </summary>
        public static bool InDocker => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    }
}
