namespace FIAP.CloudGames.Domain.Exceptions;
public class AggregateValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public AggregateValidationException(List<string> errors)
        : base("Multiple validation errors occurred.")
    {
        Errors = errors.AsReadOnly();
    }
}