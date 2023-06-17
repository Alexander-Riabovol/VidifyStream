using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    /// <summary>
    /// Represents a database context for managing and interacting with the entities using Entity Framework Core.
    /// </summary>
    public class DataContext : DbContext
    {
        // .ctor that meets the requirements of the Dependency Injection
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Applying IEntityTypeConfiguration classes which helps ef core to setup Models
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Video> Videos => Set<Video>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Playlist> Playlists => Set<Playlist>();
        public DbSet<ViewData> ViewData => Set<ViewData>();
        public DbSet<Notification> Notifications => Set<Notification>();
    }
}
