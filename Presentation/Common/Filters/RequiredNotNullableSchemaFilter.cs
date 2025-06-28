using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Presentation.Common.Filters;

public class RequiredNotNullableSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null) return;

        var requiredProperties = new HashSet<string>();

        foreach (var property in schema.Properties)
        {
            var propertyInfo = context.Type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => 
                    string.Equals(p.Name, property.Key, StringComparison.OrdinalIgnoreCase));

            if (propertyInfo != null)
            {
                var isNullable = IsPropertyNullable(propertyInfo, context.Type);
                
                if (!isNullable)
                {
                    requiredProperties.Add(property.Key);
                    property.Value.Nullable = false;
                }
            }
        }

        if (requiredProperties.Any())
        {
            schema.Required = requiredProperties;
        }
    }

    private static bool IsPropertyNullable(PropertyInfo propertyInfo, Type type)
    {
        var nullableContext = new NullabilityInfoContext();
        var nullabilityInfo = nullableContext.Create(propertyInfo);
        
        if (nullabilityInfo.WriteState == NullabilityState.NotNull)
            return false;

        if (type.IsRecord())
        {
            var constructors = type.GetConstructors();
            var primaryConstructor = constructors
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();

            if (primaryConstructor != null)
            {
                var parameter = primaryConstructor.GetParameters()
                    .FirstOrDefault(p => 
                        string.Equals(p.Name, propertyInfo.Name, StringComparison.OrdinalIgnoreCase));

                if (parameter != null)
                {
                    if (parameter.HasDefaultValue && parameter.DefaultValue == null)
                        return true;
                    var paramNullable = new NullabilityInfoContext().Create(parameter);
                    return paramNullable.WriteState == NullabilityState.Nullable;
                }
            }
        }

        return false;
    }
}

public static class TypeExtensions
{
    public static bool IsRecord(this Type type)
    {
        return type.GetMethod("<Clone>$") != null || 
               type.GetMethods().Any(m => m.Name == "<Clone>$");
    }
}