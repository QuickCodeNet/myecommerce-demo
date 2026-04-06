using System.Reflection;
using Dapper;
using QuickCode.MyecommerceDemo.Common.Extensions;

namespace QuickCode.MyecommerceDemo.Common.Mappers;
public static class DapperTypeMapper
{
    public static void ConfigureTypeMappings()
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a =>
                a.GetName().Name!.StartsWith("QuickCode.") &&
                a.GetName().Name!.EndsWith(".Application"));

        var types = assembly!.GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.IsPublic &&
                t.Namespace != null &&
                t.Namespace.EndsWith(".Application.Dtos", StringComparison.Ordinal));

        foreach (var type in types)
        {
            SqlMapper.SetTypeMap(type,
                new CustomPropertyTypeMap(type,
                    (t, columnName) =>
                        t.GetProperties().FirstOrDefault(prop =>
                            prop.Name.Equals(columnName.GetPascalCase(), StringComparison.OrdinalIgnoreCase))!
                )
            );
        }
    }
}