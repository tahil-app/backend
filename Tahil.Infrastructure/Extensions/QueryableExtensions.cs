using System.ComponentModel;
using System.Text.Json;

namespace Tahil.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IEnumerable<QueryFilterParams>? filters)
    {
        if (filters == null || !filters.Any())
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? combined = null;

        foreach (var filter in filters)
        {
            if (string.IsNullOrWhiteSpace(filter.ColumnName) || filter.ColumnValue == null)
                continue;

            Expression property = parameter;
            foreach (var prop in filter.ColumnName.Split('.'))
            {
                property = Expression.PropertyOrField(property, prop);
            }

            var propertyType = property.Type;
            var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            object? targetValue;

            if (filter.ColumnValue is JsonElement jsonElement)
            {
                // Handle deserialized JSON values
                targetValue = JsonSerializer.Deserialize(jsonElement.GetRawText(), targetType);
            }
            else if (filter.ColumnValue is IEnumerable<object> list && filter.Operator == FilterOperators.In)
            {
                // We'll handle this in the 'In' branch
                targetValue = filter.ColumnValue;
            }
            else if (filter.ColumnValue is IConvertible)
            {
                targetValue = Convert.ChangeType(filter.ColumnValue, targetType);
            }
            else
            {
                // Try using a type converter
                var converter = TypeDescriptor.GetConverter(targetType);
                targetValue = converter.ConvertFromInvariantString(filter.ColumnValue?.ToString());
            }

            // Create constant with the correct type to match the property type
            Expression constant;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // For nullable types, we need to handle the conversion properly
                if (targetValue == null)
                {
                    constant = Expression.Constant(null, propertyType);
                }
                else
                {
                    // Convert the value to the underlying type first, then create nullable
                    var underlyingValue = Convert.ChangeType(targetValue, targetType);
                    constant = Expression.Convert(Expression.Constant(underlyingValue, targetType), propertyType);
                }
            }
            else
            {
                constant = Expression.Constant(targetValue, propertyType);
            }

            Expression filterExpression;

            switch (filter.Operator)
            {
                case FilterOperators.Equals:
                    filterExpression = Expression.Equal(property, constant);
                    break;

                case FilterOperators.Contains:
                    if (property.Type != typeof(string))
                        throw new NotSupportedException("Contains only supports string properties");
                    filterExpression = Expression.Call(property, nameof(string.Contains), null, constant);
                    break;

                case FilterOperators.In:
                    var elementType = property.Type;
                    var values = (filter.ColumnValue as IEnumerable<object>)?.Cast<object>()
                        .Select(val => Convert.ChangeType(val, elementType)).ToList();

                    if (values == null)
                        throw new InvalidOperationException("Invalid 'In' filter values");

                    var valueArray = Expression.Constant(values);
                    filterExpression = Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), new[] { elementType }, valueArray, property);
                    break;

                default:
                    throw new NotSupportedException($"Filter operator {filter.Operator} not supported");
            }

            combined = combined == null ? filterExpression : Expression.AndAlso(combined, filterExpression);
        }

        if (combined == null)
            return query;

        var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
        return query.Where(lambda);
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, QuerySortParamsModel? sort)
    {
        if (sort == null || string.IsNullOrWhiteSpace(sort.ColumnName))
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression property = parameter;

        foreach (var prop in sort.ColumnName.Split('.'))
        {
            property = Expression.PropertyOrField(property, prop);
        }

        var lambda = Expression.Lambda(property, parameter);
        var methodName = sort.Direction == SortDirection.Desc ? "OrderByDescending" : "OrderBy";

        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type);

        return (IQueryable<T>)method.Invoke(null, new object[] { query, lambda })!;
    }

}