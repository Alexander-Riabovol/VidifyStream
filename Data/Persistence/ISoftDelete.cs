namespace Data.Persistence
{
    public interface ISoftDelete
    {
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
