using Microsoft.EntityFrameworkCore;
using ResetService.Core.Concurrency;

namespace ResetService.Infrastructure.Persistence.Concurrency;

public static class ConcurrencyConflictTranslator
{
    public static ConcurrencyConflict Translate(DbUpdateConcurrencyException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return new ConcurrencyConflict();
    }
}
