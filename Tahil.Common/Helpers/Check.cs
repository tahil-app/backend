//using System.Numerics;

//namespace Tahil.Common.Helpers;

//public static class Check
//{
//    public static void IsNull<T>(T entity, string? name = default) where T : notnull
//    {
//        if (entity == null)
//            throw new DomainException($"{name ?? nameof(T)} cannot be null");
//    }

//    public static void IsValidId(int id, string name)
//    {
//        if (id < 0)
//            throw new DomainException($"{name} must be greater than zero");
//    }

//    public static void IsPositive<T>(T value, string paramName) where T : struct, INumber<T>
//    {
//        if (value < T.Zero)
//            throw new DomainException($"{paramName} must be positive");
//    }
//}