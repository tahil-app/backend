namespace Tahil.Common.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string entity) : base($"{entity} not found")
    {
    }

    public NotFoundException(string name, object key) : base($"Entity \"{name}\" {(key is not null ? $"({key})" : string.Empty)} is not found")
    {
    }
}
