namespace ResetService.Core.Concurrency;

public interface IVersionedEntity
{
    long Version { get; set; }
}
