using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Service.ManagerVPS.Extensions.DbContext
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is null) return result;
            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: Microsoft.EntityFrameworkCore.EntityState.Deleted, Entity: ISoftDeleteEntity deleteEntity }) continue;
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                deleteEntity.IsDeleted = true;

            }
            return result;
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null) return ValueTask.FromResult(result);
            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: Microsoft.EntityFrameworkCore.EntityState.Deleted, Entity: ISoftDeleteEntity deleteEntity }) continue;
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                deleteEntity.IsDeleted = true;

            }
            return ValueTask.FromResult(result);
        }
    }
}
