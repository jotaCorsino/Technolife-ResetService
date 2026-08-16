namespace ResetService.Core.Concurrency;

public sealed class ConcurrencyConflict
{
    public const string DefaultCode = "concurrency_conflict";

    public const string DefaultMessage =
        "Os dados foram alterados por outro usuário. Atualize as informações e revise antes de tentar novamente.";

    public string Code => DefaultCode;

    public string Message => DefaultMessage;
}
