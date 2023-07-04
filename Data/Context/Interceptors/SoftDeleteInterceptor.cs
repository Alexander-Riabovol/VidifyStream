using Data.Models;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Data.Context.Interceptors
{
    internal class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
                                                              InterceptionResult<int> result)
        {
            if (eventData.Context is null) return result;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete deleted }) continue;
                entry.State = EntityState.Modified;
                deleted.DeletedAt = DateTimeOffset.Now;
            }
            return result;
        }

#pragma warning disable CS1998 // 
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return SavingChanges(eventData, result);
        }
    }
}
