using System.Diagnostics;
using System.Reflection;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using QuickCode.MyecommerceDemo.Common.Extensions;
using QuickCode.MyecommerceDemo.IdentityModule.Persistence.Contexts;
using QuickCode.MyecommerceDemo.IdentityModule.Persistence.Migrations;


namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Seed;

public static class DatabaseSeeder
{
    private static Dictionary<string, List<string>> UniqueColumnMap => new()
    {
        { "Modules", ["Name"] },
        { "Models", ["Name", "ModuleName"] },
        { "ApiMethodDefinitions", ["Key"] },
        { "PortalPageDefinitions", ["Key"] },
        { "ApiMethodAccessGrants", ["ApiMethodDefinitionKey", "PermissionGroupName"] },
        { "PortalPageAccessGrants", ["PortalPageDefinitionKey", "PermissionGroupName", "PageAction"] },
        { "PortalMenus", ["Key"] },
        { "TableComboboxSettings", ["TableName", "IdColumn"] }
    };
    
    private static readonly Assembly DomainAssembly =
        AppDomain.CurrentDomain.GetAssemblies()
            .First(a => a.GetName().Name!.EndsWith(".Domain"));
    

    private static async Task<Dictionary<string, List<Dictionary<string, object>>>> ReadDataFromMigrationFilesAsync(ILogger<WriteDbContext> logger)
    {
        var fileList = typeof(BaseData).GetMigrationDataFiles();
        var models = new Dictionary<string, List<Dictionary<string, object>>>();

        foreach (var filePath in fileList)
        {
            if (!File.Exists(filePath))
            {
                logger.LogWarning("Migration file not found: {FilePath}", filePath);
                continue;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var jsonObject = JObject.Parse(json);
            var fileModels = jsonObject.ToObject<Dictionary<string, List<Dictionary<string, object>>>>();
            if (fileModels is null) continue;

            foreach (var kvp in fileModels)
            {
                if (!models.ContainsKey(kvp.Key))
                {
                    models[kvp.Key] = [];
                }

                models[kvp.Key].AddRange(kvp.Value);
            }
        }

        return models;
    }

    private static async Task<Dictionary<string, List<Dictionary<string, object>>>> CompareAndPrepareInsertDataAsync(ILogger<WriteDbContext> logger, Dictionary<string, List<Dictionary<string, object>>> currentDbData)
    {
        var result = new Dictionary<string, List<Dictionary<string, object>>>();

        var migrationFilesData = await ReadDataFromMigrationFilesAsync(logger);
        
        foreach (var (tableName, uniqueColumns) in UniqueColumnMap)
        {
            var rowsToInsert = FindMissingRowsByUniqueKeys(currentDbData, migrationFilesData, tableName, uniqueColumns);

            if (rowsToInsert.Count == 0)
            {
                logger.LogInformation("No Data for table: {Table}", tableName);
                continue;
            }
            
            result[tableName] = rowsToInsert;
        }
        
        return result;
    }

    public static async Task SeedAsync(WriteDbContext dbContext, SeedRepository seedRepository)
    {
        const int maxRetries = 3;
        var logger = seedRepository._logger;
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                
                if (attempt > 1)
                {
                    logger.LogWarning("🔄 Retrying database seeding (Attempt {Attempt}/{MaxRetries})...", attempt, maxRetries);
                }
                else
                {
                    logger.LogInformation("🌱 Starting database seeding...");
                }
                
                var currentDbData = await seedRepository.FetchCurrentDataFromDatabaseAsync();
                
                var cleanupStopwatch = Stopwatch.StartNew();
                await TryCleanupAsync(logger, currentDbData, seedRepository);
                cleanupStopwatch.Stop();
                
                currentDbData = await seedRepository.FetchCurrentDataFromDatabaseAsync();
                
                var insertData = await CompareAndPrepareInsertDataAsync(logger, currentDbData);
                var seedResults = await InsertDataAsync(dbContext, insertData, logger);

                stopwatch.Stop();
                
                if (cleanupStopwatch.ElapsedMilliseconds > 0)
                {
                    logger.LogInformation("🧹 Cleanup completed in {ElapsedMilliseconds} ms ({ElapsedSeconds:F2} seconds)", 
                        cleanupStopwatch.ElapsedMilliseconds, cleanupStopwatch.Elapsed.TotalSeconds);
                }
                
                if (seedResults.Count > 0)
                {
                    logger.LogInformation("📊 Seeded tables summary:");
                    foreach (var result in seedResults)
                    {
                        logger.LogInformation("  • {Table}: {InsertedRow} row(s) inserted", result.TableName, result.InsertedRows);
                    }
                }
                
                logger.LogInformation("✅ Migration data imported successfully.");
                logger.LogInformation("⏱️ Database seeding completed in {ElapsedMilliseconds} ms ({ElapsedSeconds:F2} seconds)", 
                    stopwatch.ElapsedMilliseconds, stopwatch.Elapsed.TotalSeconds);
                
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Database seeding failed on attempt {Attempt}/{MaxRetries}", attempt, maxRetries);
                
                if (attempt == maxRetries)
                {
                    logger.LogError("❌ Database seeding failed after {MaxRetries} attempts. Giving up.", maxRetries);
                    throw;
                }
                
                var delayMs = attempt * 500;
                logger.LogInformation("⏳ Waiting {DelayMs} ms before retry...", delayMs);
                await Task.Delay(delayMs);
            }
        }
    }

    private static async Task TryCleanupAsync(ILogger<WriteDbContext> logger, Dictionary<string, List<Dictionary<string, object>>> currentDbData, SeedRepository seedRepository)
    {
        try
        {
            await CleanupAsync(currentDbData, seedRepository, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cleanup failed, but continuing with seed operation");
        }
    }

    private static async Task<List<(string TableName, int InsertedRows)>> InsertDataAsync(
        WriteDbContext dbContext, 
        Dictionary<string, List<Dictionary<string, object>>> insertData, 
        ILogger<WriteDbContext> logger)
    {
        var results = new List<(string TableName, int InsertedRows)>();
        
        foreach (var tableName in insertData.Keys)
        {
            var insertedRows = await InsertTableDataAsync(dbContext, tableName, insertData[tableName], logger);
            if (insertedRows > 0)
            {
                results.Add((tableName, insertedRows));
            }
        }
        
        return results;
    }

    private static async Task<int> InsertTableDataAsync(
        WriteDbContext dbContext, 
        string tableName, 
        List<Dictionary<string, object>> tableData, 
        ILogger<WriteDbContext> logger)
    {
        var entityType = DomainAssembly.GetType($"{DomainAssembly.GetName().Name}.Entities.{tableName.GetPascalCase().Singularize()}");
        if (entityType is null)
        {
            logger.LogWarning("Entity type not found for table: {Table}", tableName);
            return 0;
        }

        var insertRows = tableData.DeserializeToList(entityType);
        if (insertRows.Count == 0)
        {
            logger.LogInformation("No rows to insert for table: {Table}", tableName);
            return 0;
        }

        var dbSet = dbContext.GetType()
            .GetMethod("Set", Type.EmptyTypes)!
            .MakeGenericMethod(entityType)
            .Invoke(dbContext, null);

        var addRangeMethod = dbSet!.GetType()
            .GetMethod("AddRange", [typeof(IEnumerable<>).MakeGenericType(entityType)]);
        
        if (addRangeMethod == null)
        {
            logger.LogWarning("AddRange method not found for table: {Table}", tableName);
            return 0;
        }

        try
        {
            addRangeMethod.Invoke(dbSet, [insertRows]);
            
            var connection = dbContext.Database.GetDbConnection();
            if (string.IsNullOrEmpty(connection.ConnectionString))
            {
                if (dbContext is Microsoft.EntityFrameworkCore.Infrastructure.IInfrastructure<IServiceProvider> infrastructure)
                {
                    var serviceProvider = infrastructure.Instance;
                    var options = serviceProvider.GetService<Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptions>();
                    if (options != null)
                    {
                        var extension = options.Extensions.OfType<Microsoft.EntityFrameworkCore.Infrastructure.RelationalOptionsExtension>().FirstOrDefault();
                        if (extension?.ConnectionString != null)
                        {
                            connection.ConnectionString = extension.ConnectionString;
                        }
                    }
                }
            }
            
            await dbContext.SaveChangesAsync();
            
            return insertRows.Count;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "DbUpdateException on table {Table}", tableName);
            return 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception on table {Table}", tableName);
            return 0;
        }
    }

    private static List<Dictionary<string, object>> ConvertJsonToDictionary(string json)
    {
        var parsedJson = JArray.Parse(json);
        var dictionaries = parsedJson.ToObject<List<Dictionary<string, object>>>()!;
        
        foreach (var dict in dictionaries)
        {
            ConvertEnumIntsToStrings(dict);
        }
        
        return dictionaries;
    }

    private static void ConvertEnumIntsToStrings(Dictionary<string, object> dict)
    {
        foreach (var key in dict.Keys.ToList())
        {
            if (dict[key] is int intVal)
            {
                var enumType = GetEnumTypeForIntValue(intVal);
                if (enumType != null)
                {
                    var enumName = Enum.GetName(enumType, intVal);
                    if (enumName != null)
                    {
                        dict[key] = enumName;
                    }
                }
            }
        }
    }

    private static List<Dictionary<string, object>> FindMissingRowsByUniqueKeys(
        Dictionary<string, List<Dictionary<string, object>>> existingModel,
        Dictionary<string, List<Dictionary<string, object>>> newModel,
        string tableName,
        List<string>? uniqueColumns = null)
    {
        var existingRows = existingModel.GetValueOrDefault(tableName, []);
        var newRows = newModel.GetValueOrDefault(tableName, []);
        var insertCandidates = new List<Dictionary<string, object>>();
        
        if (existingRows.Count == 0)
        {
            return newRows;
        }

        foreach (var newRow in newRows)
        {
            bool exists = uniqueColumns is null || uniqueColumns.Count == 0
                ? existingRows.Any(existing => AreDictionariesEqual(existing, newRow))
                : existingRows.Any(existing => 
                    uniqueColumns.All(column => AreValuesEqual(
                        existing.GetValueOrDefault(column),
                        newRow.GetValueOrDefault(column))));

            if (!exists)
                insertCandidates.Add(newRow);
        }

        return insertCandidates;
    }

    private static bool AreDictionariesEqual(Dictionary<string, object> dict1, Dictionary<string, object> dict2)
    {
        if (dict1.Count != dict2.Count)
            return false;

        foreach (var kvp in dict1)
        {
            if (!dict2.ContainsKey(kvp.Key))
                return false;

            if (!AreValuesEqual(kvp.Value, dict2[kvp.Key]))
                return false;
        }

        return true;
    }

    private static bool AreValuesEqual(object? value1, object? value2)
    {
        if (value1 == null && value2 == null)
            return true;

        if (value1 == null || value2 == null)
            return false;

        if (value1 is int intVal1 && value2 is string strVal2)
        {
            var enumType = GetEnumTypeForIntValue(intVal1);
            if (enumType != null)
            {
                var enumName = Enum.GetName(enumType, intVal1);
                return string.Equals(enumName, strVal2.Trim(), StringComparison.OrdinalIgnoreCase);
            }
        }

        if (value1 is string strVal1 && value2 is int intVal2)
        {
            var enumType = GetEnumTypeForIntValue(intVal2);
            if (enumType != null)
            {
                var enumName = Enum.GetName(enumType, intVal2);
                return string.Equals(strVal1.Trim(), enumName, StringComparison.OrdinalIgnoreCase);
            }
        }

        var str1 = NormalizeValue(value1);
        var str2 = NormalizeValue(value2);

        return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeValue(object value)
    {
        if (value == null)
            return string.Empty;

        if (value is string str)
            return str.Trim();

        if (value is bool boolVal)
            return boolVal ? "true" : "false";

        if (value is DateTime dateTime)
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");

        if (value is int intVal)
        {
            var enumType = GetEnumTypeForIntValue(intVal);
            if (enumType != null)
            {
                return Enum.GetName(enumType, intVal) ?? intVal.ToString();
            }
            return intVal.ToString();
        }

        if (value.GetType().IsEnum)
        {
            return value.ToString() ?? string.Empty;
        }

        return value.ToString()?.Trim() ?? string.Empty;
    }

    private static Type? GetEnumTypeForIntValue(int intVal)
    {
        var enumTypes = DomainAssembly
            .GetTypes()
            .Where(t => t.IsEnum)
            .ToList();

        foreach (var enumType in enumTypes)
        {
            if (Enum.IsDefined(enumType, intVal))
            {
                return enumType;
            }
        }

        return null;
    }

    #region Cleanup Methods

    private static async Task CleanupAsync(Dictionary<string, List<Dictionary<string, object>>> currentDbData, SeedRepository seedRepository, ILogger<WriteDbContext> logger)
    {
        try
        {
            logger.LogInformation("🧹 Starting cleanup process...");

            var migrationFilesData = await ReadDataFromMigrationFilesAsync(logger);

            var (modulesToDelete, modelsToDelete) = FindModulesAndModelsToDelete(currentDbData, migrationFilesData);
            
            if (modulesToDelete.Count == 0 && modelsToDelete.Count == 0)
            {
                logger.LogInformation("✅ No modules or models to delete. Cleanup skipped.");
                return;
            }

            logger.LogInformation("Found {ModuleCount} modules and {ModelCount} models to delete", modulesToDelete.Count, modelsToDelete.Count);

            await seedRepository.ClearSystemGrantsAsync();
            await seedRepository.DeletePortalMenusForDeletedModulesAsync(modulesToDelete);
            await seedRepository.DeletePortalMenusForDeletedModelsAsync(modelsToDelete);
            
            await seedRepository.DeleteTopicWorkflowsForDeletedModulesAndModelsAsync(modulesToDelete, modelsToDelete, currentDbData);
            await seedRepository.CleanKafkaEventsForDeletedModulesAndModelsAsync(modulesToDelete, modelsToDelete);
            await seedRepository.DeleteDefinitionsForDeletedModulesAndModelsAsync(modulesToDelete, modelsToDelete, currentDbData);
            await seedRepository.DeleteModelsAndModulesAsync(modulesToDelete, modelsToDelete);

            logger.LogInformation("✅ Cleanup completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Cleanup failed with error.");
        }
    }

    private static (List<string> modulesToDelete, List<(string Name, string ModuleName)> modelsToDelete) FindModulesAndModelsToDelete(
        Dictionary<string, List<Dictionary<string, object>>> currentDbData,
        Dictionary<string, List<Dictionary<string, object>>> migrationFilesData)
    {
        var migrationModules = migrationFilesData.GetValueOrDefault("Modules", []).Select(m => m["Name"]?.ToString() ?? "").Where(n => !string.IsNullOrEmpty(n)).ToHashSet();
        var migrationModels = migrationFilesData.GetValueOrDefault("Models", [])
            .Select(m => (Name: m["Name"]?.ToString() ?? "", ModuleName: m["ModuleName"]?.ToString() ?? ""))
            .Where(m => !string.IsNullOrEmpty(m.Name) && !string.IsNullOrEmpty(m.ModuleName))
            .ToHashSet();

        var dbModules = currentDbData.GetValueOrDefault("Modules", []).Select(m => m["Name"]?.ToString() ?? "").Where(n => !string.IsNullOrEmpty(n)).ToList();
        var dbModels = currentDbData.GetValueOrDefault("Models", [])
            .Select(m => (Name: m["Name"]?.ToString() ?? "", ModuleName: m["ModuleName"]?.ToString() ?? ""))
            .Where(m => !string.IsNullOrEmpty(m.Name) && !string.IsNullOrEmpty(m.ModuleName))
            .ToList();

        var modulesToDelete = dbModules.Where(m => !migrationModules.Contains(m)).ToList();
        var modelsToDelete = dbModels.Where(m => !migrationModels.Contains(m)).ToList();

        return (modulesToDelete, modelsToDelete);
    }

    #endregion
}