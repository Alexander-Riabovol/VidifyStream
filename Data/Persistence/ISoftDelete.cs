namespace Data.Persistence
{
    /// <summary>
    /// The <see cref="ISoftDelete"/> interface defines a contract
    /// for entities that support soft delete functionality.
    /// </summary>
    internal interface ISoftDelete
    {
        /// <summary>
        /// Represents the timestamp when the entity was deleted.
        /// The property is nullable to accommodate entities that have not been deleted.
        /// </summary>
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
