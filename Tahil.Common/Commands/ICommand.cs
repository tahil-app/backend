namespace Tahil.Common.Commands;

public interface ICommand : ICommand<Unit>;

public interface ICommand<out TResponse>
    : IRequest<TResponse>
    where TResponse : notnull;
