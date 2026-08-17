using Microsoft.AspNetCore.Identity;
using ResetService.Core.Concurrency;

namespace ResetService.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>, IVersionedEntity
{
    public string DisplayName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public bool MustChangePassword { get; set; }

    public DateTime? LastLoginAtUtc { get; set; }

    public long Version { get; set; }
}
