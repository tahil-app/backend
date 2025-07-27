﻿namespace Tahil.Common.Commands;

public interface ICommandHandler<TCommand>
    : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand;

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull;