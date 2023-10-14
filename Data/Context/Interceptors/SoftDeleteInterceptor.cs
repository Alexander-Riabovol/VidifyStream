using VidifyStream.Data.Models;
using VidifyStream.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace VidifyStream.Data.Context.Interceptors
{
    /// <summary>
    /// Interceptor for implementing soft delete functionality during database save changes.
    /// </summary>
    internal class SoftDeleteInterceptor : SaveChangesInterceptor
    {

        /// <summary>
        /// Overrides the SavingChanges method to perform soft delete logic synchronously.
        /// </summary>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
                                                              InterceptionResult<int> result)
        {
            return SavingChangesAsync(eventData, result).Result;
        }

        /// <summary>
        /// Overrides the SavingChangesAsync method to perform soft delete logic asynchronously.
        /// </summary>
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null) return result;

            var entitiesToDelete = new List<object>();

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete deleted }) continue;

                entry.State = EntityState.Modified;
                deleted.DeletedAt = DateTimeOffset.Now;

                // Add Cascade Soft Delete for User & Video models
                var context = entry.Context as DataContext;
                if (deleted is User deletedUser)
                {
                    var videos = await context!.Videos.Where(v => v.UserId == deletedUser.UserId).ToListAsync();
                    var comments = await context!.Comments.Where(c => c.UserId == deletedUser.UserId).ToListAsync();

                    entitiesToDelete.AddRange(videos);
                    entitiesToDelete.AddRange(comments);
                }
                else if(deleted is Video deletedVideo)
                {
                    var comments = await context!.Comments.Where(c => c.VideoId == deletedVideo.VideoId).ToListAsync();

                    entitiesToDelete.AddRange(comments);
                }
                else if(deleted is Comment deletedComment)
                {
                    var replies = await context!.Comments.Where(c => c.RepliedToId == deletedComment.CommentId).ToListAsync();

                    entitiesToDelete.AddRange(replies);
                }
            }

            if(entitiesToDelete.Count > 0)
            {
                eventData.Context.RemoveRange(entitiesToDelete);
                await eventData.Context.SaveChangesAsync();
            }

            return result;
        }
    }
}
