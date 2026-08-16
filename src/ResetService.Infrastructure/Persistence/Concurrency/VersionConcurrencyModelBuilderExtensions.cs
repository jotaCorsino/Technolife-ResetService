using Microsoft.EntityFrameworkCore;
using ResetService.Core.Concurrency;

namespace ResetService.Infrastructure.Persistence.Concurrency;

public static class VersionConcurrencyModelBuilderExtensions
{
    public static ModelBuilder ConfigureVersionConcurrency(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        var versionedEntityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(entityType => typeof(IVersionedEntity).IsAssignableFrom(entityType.ClrType));

        foreach (var entityType in versionedEntityTypes)
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property<long>(nameof(IVersionedEntity.Version))
                .IsConcurrencyToken();
        }

        return modelBuilder;
    }
}
