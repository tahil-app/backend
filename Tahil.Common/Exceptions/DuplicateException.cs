namespace Tahil.Common.Exceptions;

public class DuplicateException : DomainException
{
    public DuplicateException(string message) : base(message)
    {
    }
}
