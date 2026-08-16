using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ResetService.Core.Concurrency;

namespace ResetService.Infrastructure.Persistence.Concurrency;

public sealed class VersionConcurrencyInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        SetVersions(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SetVersions(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void SetVersions(DbContext? dbContext)
    {
        if (dbContext is null)
        {
            return;
        }

        foreach (var entry in dbContext.ChangeTracker.Entries<IVersionedEntity>())
        {
            var versionProperty = entry.Property(entity => entity.Version);

            switch (entry.State)
            {
                case EntityState.Added:
                    versionProperty.CurrentValue = 1;
                    break;
                case EntityState.Modified:
                    versionProperty.CurrentValue = checked(versionProperty.OriginalValue + 1);
                    break;
            }
        }
    }
}
