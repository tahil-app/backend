namespace Tahil.Common.Queries;

public interface IQuery<TResponse>
    : IRequest<TResponse>
    where TResponse : notnull;