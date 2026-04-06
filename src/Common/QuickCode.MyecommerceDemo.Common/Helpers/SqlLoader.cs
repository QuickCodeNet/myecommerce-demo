using System.Collections.Concurrent;
using System.Reflection;

namespace QuickCode.MyecommerceDemo.Common.Helpers;

public static class SqlLoader
{
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    public static string Load(string resourcePath)
    {
        if (Cache.TryGetValue(resourcePath, out var sql))
        {
            return sql;
        }

        var moduleName = resourcePath.Contains('.') ? resourcePath.Split('.')[0] : resourcePath;
        
        if (moduleName.Equals(resourcePath))
        {
            throw new ArgumentException("Resource path must contain a module name followed by a dot.");
        }

        var moduleAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .First(i => i.FullName!.Contains($"{moduleName}.Persistence"));

        var resourceItemName = resourcePath.Replace(moduleName, $"{moduleName}.Persistence.Sql");
        var fullResourceName = moduleAssembly
            .GetManifestResourceNames()
            .FirstOrDefault(r => r.EndsWith(resourceItemName));

        if (fullResourceName == null)
        {
            throw new FileNotFoundException($"Embedded SQL resource not found: {resourcePath}");
        }

        using var stream = moduleAssembly.GetManifestResourceStream(fullResourceName);
        using var reader = new StreamReader(stream!);
        sql = reader.ReadToEnd();
        Cache[resourcePath] = sql;

        return sql;
    }
}