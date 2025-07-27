using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tahil.Infrastructure.Helpers;

public static class ColumnJsonConverter
{
    public static ValueConverter<T, string> Convert<T>() where T : notnull, new()
    {
        return new ValueConverter<T, string>(
            v => JsonConvert.SerializeObject(v, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            }),
            v => JsonConvert.DeserializeObject<T>(v, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            }));
    }
}