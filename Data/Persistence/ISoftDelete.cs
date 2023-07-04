namespace Data.Persistence
{
    internal interface ISoftDelete
    {
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
