namespace Tahil.Common.Exceptions;

public class DuplicateException : DomainException
{
    public DuplicateException(string entity) : base($"{entity} is duplicated")
    {
    }
}
