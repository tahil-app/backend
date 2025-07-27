namespace Tahil.Common.Behaviors;

public class LoggingBehavoir<TRequest, TResponse>
    (ILogger<LoggingBehavoir<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? "no-trace";
        var requestName = typeof(TRequest).Name;
        var responseName = typeof(TResponse).Name;
        var stopwatch = Stopwatch.StartNew();

        var sanitized = SanitizeRequest(request);

        logger.LogInformation("[START] Handle Request = {RequestName} - TraceId = {TraceId} - Payload = {@Payload}",
            requestName, traceId, sanitized);

        var response = await next();

        stopwatch.Stop();

        var timeTaken = stopwatch.Elapsed;
        if (timeTaken.Seconds > 3)
            logger.LogWarning("[PERFORMANCE] The request {RequestName} took {TimeTaken}",
                requestName, timeTaken.Seconds);

        logger.LogInformation("[END] Handled Request = {RequestName} with Response = {Reponse} - TraceId = {TraceId}",
            requestName, SanitizeRequest(response), traceId);

        return response;
    }

    private static object SanitizeRequest<T>(T request)
    {
        if (request == null) return null;

        var result = new Dictionary<string, object>();
        SanitizeObject(request, result, string.Empty);
        return result;
    }

    private static void SanitizeObject(object obj, Dictionary<string, object> result, string prefix)
    {
        if (obj == null) return;

        var type = obj.GetType();
        var props = type.GetProperties();

        foreach (var prop in props)
        {
            var propValue = prop.GetValue(obj);
            var propName = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";

            if (propName == "FormFile" || propName == "File")
            {
                continue;
            }

            if (prop.GetCustomAttribute<IgnoreLogging>() != null)
            {
                result[propName] = "***REDACTED***";
                continue;
            }

            if (type == typeof(DateOnly) || type == typeof(TimeOnly) || type == typeof(DateTime) || type == typeof(DateTimeOffset))
            {
                result[prefix] = obj.ToString();
                return;
            }
            else if (propValue == null || propValue is string || propValue.GetType().IsPrimitive)
            {
                result[propName] = propValue;
            }
            else if (propValue is IEnumerable<object> list)
            {
                int index = 0;
                foreach (var item in list)
                {
                    var itemPrefix = $"{propName}[{index}]";
                    SanitizeObject(item, result, itemPrefix);
                    index++;
                }
            }
            else if (propValue is System.Collections.IEnumerable enumerable && !(propValue is string))
            {
                int index = 0;
                foreach (var item in enumerable)
                {
                    var itemPrefix = $"{propName}[{index}]";
                    SanitizeObject(item, result, itemPrefix);
                    index++;
                }
            }
            else
            {
                SanitizeObject(propValue, result, propName);
            }
        }
    }

}