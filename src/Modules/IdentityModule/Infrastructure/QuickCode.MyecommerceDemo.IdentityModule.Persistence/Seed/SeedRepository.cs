using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Entities;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;
using QuickCode.MyecommerceDemo.IdentityModule.Persistence.Contexts;
using QuickCode.MyecommerceDemo.Common;

namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Seed;

public class SeedRepository
{
    private readonly WriteDbContext _writeContext;
    private readonly ReadDbContext _readContext;
    internal readonly ILogger<WriteDbContext> _logger;
    private readonly IApiMethodDefinitionRepository _apiMethodDefinitionRepository;
    private readonly IPortalPageDefinitionRepository _portalPageDefinitionRepository;
    private readonly IPortalMenuRepository _portalMenuRepository;
    private readonly IKafkaEventRepository _kafkaEventRepository;
    private readonly ITopicWorkflowRepository _topicWorkflowRepository;
    private readonly IApiMethodAccessGrantRepository _apiMethodAccessGrantRepository;
    private readonly IPortalPageAccessGrantRepository _portalPageAccessGrantRepository;
    private readonly IModelRepository _modelRepository;
    private readonly IModuleRepository _moduleRepository;

    public SeedRepository(WriteDbContext dbContext, IServiceProvider serviceProvider)
    {
        _writeContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _readContext = serviceProvider.GetRequiredService<ReadDbContext>();
        _logger = serviceProvider.GetRequiredService<ILogger<WriteDbContext>>();
        _apiMethodDefinitionRepository = serviceProvider.GetRequiredService<IApiMethodDefinitionRepository>();
        _portalPageDefinitionRepository = serviceProvider.GetRequiredService<IPortalPageDefinitionRepository>();
        _portalMenuRepository = serviceProvider.GetRequiredService<IPortalMenuRepository>();
        _kafkaEventRepository = serviceProvider.GetRequiredService<IKafkaEventRepository>();
        _topicWorkflowRepository = serviceProvider.GetRequiredService<ITopicWorkflowRepository>();
        _apiMethodAccessGrantRepository = serviceProvider.GetRequiredService<IApiMethodAccessGrantRepository>();
        _portalPageAccessGrantRepository = serviceProvider.GetRequiredService<IPortalPageAccessGrantRepository>();
        _modelRepository = serviceProvider.GetRequiredService<IModelRepository>();
        _moduleRepository = serviceProvider.GetRequiredService<IModuleRepository>();
    }

    public async Task<Dictionary<string, List<Dictionary<string, object>>>> FetchCurrentDataFromDatabaseAsync()
    {
        var dbModels = new Dictionary<string, List<Dictionary<string, object>>>();
        try
        {
            var modules = await _writeContext.Module.ToListAsync();
            var models = await _writeContext.Model.ToListAsync();
            var apiMethodDefinitions = await _writeContext.ApiMethodDefinition.ToListAsync();
            var portalPageDefinitions = await _writeContext.PortalPageDefinition.ToListAsync();
            var apiMethodAccessGrants = await _writeContext.ApiMethodAccessGrant.ToListAsync();
            var portalPageAccessGrants = await _writeContext.PortalPageAccessGrant.ToListAsync();
            var portalMenus = await _writeContext.PortalMenu.ToListAsync();
            var tableComboboxSettings = await _writeContext.TableComboboxSetting.ToListAsync();

            dbModels.Add("Modules", ConvertEntitiesToDictionary(modules));
            dbModels.Add("Models", ConvertEntitiesToDictionary(models));
            dbModels.Add("ApiMethodDefinitions", ConvertEntitiesToDictionary(apiMethodDefinitions));
            dbModels.Add("PortalPageDefinitions", ConvertEntitiesToDictionary(portalPageDefinitions));
            dbModels.Add("ApiMethodAccessGrants", ConvertEntitiesToDictionary(apiMethodAccessGrants));
            dbModels.Add("PortalPageAccessGrants", ConvertEntitiesToDictionary(portalPageAccessGrants));
            dbModels.Add("PortalMenus", ConvertEntitiesToDictionary(portalMenus));
            dbModels.Add("TableComboboxSettings", ConvertEntitiesToDictionary(tableComboboxSettings));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Migration data fetch failed.");
            return dbModels;
        }

        return dbModels;
    }

    private List<Dictionary<string, object>> ConvertEntitiesToDictionary<T>(List<T> entities)
    {
        var result = new List<Dictionary<string, object>>();
        foreach (var entity in entities)
        {
            var dict = new Dictionary<string, object>();
            foreach (var prop in typeof(T).GetProperties())
            {
                var value = prop.GetValue(entity);
                if (value != null)
                {
                    if (value is Enum enumValue)
                    {
                        dict[prop.Name] = enumValue.ToString();
                    }
                    else
                    {
                        dict[prop.Name] = value;
                    }
                }
            }
            result.Add(dict);
        }
        return result;
    }

    public async Task<(int apiCount, int portalCount)> ClearSystemGrantsAsync()
    {
        try
        {
            var apiResult = await _apiMethodAccessGrantRepository.ClearApiMethodAccessGrantsAsync();
            var apiCount = apiResult.Code == 0 ? apiResult.Value : 0;
            _logger.LogInformation("Cleared {Count} system ApiMethodAccessGrants", apiCount);

            var portalResult = await _portalPageAccessGrantRepository.ClearPortalPageAccessGrantsAsync();
            var portalCount = portalResult.Code == 0 ? portalResult.Value : 0;
            _logger.LogInformation("Cleared {Count} system PortalPageAccessGrants", portalCount);

            return (apiCount, portalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing system grants");
            return (0, 0);
        }
    }

    public async Task<int> DeletePortalMenusForDeletedModulesAsync(List<string> modulesToDelete)
    {
        if (modulesToDelete == null || modulesToDelete.Count == 0)
            return 0;

        int totalDeleted = 0;
        foreach (var moduleName in modulesToDelete)
        {
            try
            {
                var result = await _portalMenuRepository.DeletePortalMenuItemsWithModuleNameAsync(moduleName);
                if (result.Code == 0)
                    totalDeleted += result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error deleting PortalMenus for module {ModuleName}", moduleName);
            }
        }

        if (totalDeleted > 0)
        {
            _logger.LogInformation("Deleted {Count} PortalMenu items for deleted modules", totalDeleted);
        }

        return totalDeleted;
    }
    
    public async Task<int> DeletePortalMenusForDeletedModelsAsync(List<(string Name, string ModuleName)> modelsToDelete)
    {
        if (modelsToDelete == null || modelsToDelete.Count == 0)
            return 0;

        int totalDeleted = 0;
        foreach (var item in modelsToDelete)
        {
            try
            {
                var result = await _portalMenuRepository.DeletePortalMenuItemsWithModelNameAsync(item.ModuleName, item.Name);
                if (result.Code == 0)
                    totalDeleted += result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error deleting PortalMenus for model {ModelName} model {ModuleName}", item.Name,
                    item.ModuleName);
            }
        }

        if (totalDeleted > 0)
        {
            _logger.LogInformation("Deleted {Count} PortalMenu items for deleted models", totalDeleted);
        }

        return totalDeleted;
    }

    public async Task<int> DeleteTopicWorkflowsForDeletedModulesAndModelsAsync(
        List<string> modulesToDelete,
        List<(string Name, string ModuleName)> modelsToDelete,
        Dictionary<string, List<Dictionary<string, object>>> currentDbData)
    {
        var modulesToDeleteSet = modulesToDelete.ToHashSet();
        var modelsToDeleteSet = modelsToDelete.ToHashSet();

        var apiDefinitions = currentDbData.GetValueOrDefault("ApiMethodDefinitions", []);
        var kafkaEventsToDelete = new HashSet<string>();

        foreach (var def in apiDefinitions)
        {
            var moduleName = def["ModuleName"]?.ToString() ?? "";
            var modelName = def["ModelName"]?.ToString() ?? "";
            var key = def["Key"]?.ToString() ?? "";

            if (string.IsNullOrEmpty(key)) continue;

            if (modulesToDeleteSet.Contains(moduleName) ||
                modelsToDeleteSet.Contains((modelName, moduleName)))
            {
                try
                {
                    var kafkaEventsResponse = await _apiMethodDefinitionRepository.GetKafkaEventsApiMethodDefinitionsAsync(key);
                    if (kafkaEventsResponse.Code == 0 && kafkaEventsResponse.Value != null)
                    {
                        foreach (var kafkaEvent in kafkaEventsResponse.Value)
                        {
                            if (!string.IsNullOrEmpty(kafkaEvent.TopicName))
                                kafkaEventsToDelete.Add(kafkaEvent.TopicName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error getting KafkaEvents for ApiMethodDefinition {Key}", key);
                }
            }
        }

        int totalDeleted = 0;
        foreach (var topicName in kafkaEventsToDelete)
        {
            try
            {
                var workflowsResponse = await _kafkaEventRepository.GetTopicWorkflowsKafkaEventsAsync(topicName);
                if (workflowsResponse.Code == 0 && workflowsResponse.Value != null)
                {
                    foreach (var workflow in workflowsResponse.Value)
                    {
                        try
                        {
                            var workflowDto = await _topicWorkflowRepository.GetByPkAsync(workflow.Id);
                            if (workflowDto.Code == 0 && workflowDto.Value != null)
                            {
                                EnsureConnectionString(_writeContext);
                                var deleteResult = await _topicWorkflowRepository.DeleteAsync(workflowDto.Value);
                                if (deleteResult.Code == 0 && deleteResult.Value)
                                    totalDeleted++;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Error deleting TopicWorkflow {Id} for KafkaEvent {TopicName}", workflow.Id, topicName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error getting TopicWorkflows for KafkaEvent {TopicName}", topicName);
            }
        }

        if (totalDeleted > 0)
        {
            _logger.LogInformation("Soft deleted {Count} TopicWorkflows for deleted modules/models", totalDeleted);
        }

        return totalDeleted;
    }

    public void EnsureConnectionString(DbContext dbContext)
    {
        var connection = dbContext.Database.GetDbConnection();
        
        if (!string.IsNullOrEmpty(connection.ConnectionString))
        {
            return;
        }
        
        if (dbContext is IInfrastructure<IServiceProvider> infrastructure)
        {
            var serviceProvider = infrastructure.Instance;
            var options = serviceProvider.GetService<IDbContextOptions>();
            if (options != null)
            {
                var extension = options.Extensions.OfType<RelationalOptionsExtension>().FirstOrDefault();
                if (extension?.ConnectionString != null)
                {
                    connection.ConnectionString = extension.ConnectionString;
                }
            }
        }
    }
    
    public async Task<int> CleanKafkaEventsForDeletedModulesAndModelsAsync(
        List<string> modulesToDelete,
        List<(string Name, string ModuleName)> modelsToDelete)
    {
        int totalDeleted = 0;

        foreach (var moduleName in modulesToDelete)
        {
            try
            {
                var result = await _kafkaEventRepository.CleanKafkaEventsWithModuleNameAsync(moduleName);
                if (result.Code == 0)
                    totalDeleted += result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error cleaning KafkaEvents for module {ModuleName}", moduleName);
            }
        }

        foreach (var (modelName, moduleName) in modelsToDelete)
        {
            try
            {
                var result = await _kafkaEventRepository.CleanKafkaEventsWithModelNameAsync(modelName);
                if (result.Code == 0)
                    totalDeleted += result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error cleaning KafkaEvents for model {ModelName} in module {ModuleName}", modelName, moduleName);
            }
        }

        if (totalDeleted > 0)
        {
            _logger.LogInformation("Cleaned {Count} KafkaEvents for deleted modules/models", totalDeleted);
        }

        return totalDeleted;
    }

    public async Task DeleteDefinitionsForDeletedModulesAndModelsAsync(
        List<string> modulesToDelete,
        List<(string Name, string ModuleName)> modelsToDelete,
        Dictionary<string, List<Dictionary<string, object>>> currentDbData)
    {
        int totalApiDeleted = 0;
        int totalPortalDeleted = 0;

        foreach (var moduleName in modulesToDelete)
        {
            try
            {
                var apiResult = await _apiMethodDefinitionRepository.DeleteApiMethodDefinitionsWithModuleNameAsync(moduleName);
                if (apiResult.Code == 0)
                    totalApiDeleted += apiResult.Value;

                var portalResult = await _portalPageDefinitionRepository.DeletePortalPageDefinitionsWithModuleNameAsync(moduleName);
                if (portalResult.Code == 0)
                    totalPortalDeleted += portalResult.Value;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error deleting definitions for module {ModuleName}", moduleName);
            }
        }

        foreach (var (modelName, moduleName) in modelsToDelete)
        {
            try
            {
                var apiResult = await _apiMethodDefinitionRepository.DeleteApiMethodDefinitionsWithModelNameAsync(modelName);
                if (apiResult.Code == 0)
                    totalApiDeleted += apiResult.Value;

                var portalResult = await _portalPageDefinitionRepository.DeletePortalPageDefinitionsWithModelNameAsync(modelName);
                if (portalResult.Code == 0)
                    totalPortalDeleted += portalResult.Value;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error deleting definitions for model {ModelName} in module {ModuleName}", modelName, moduleName);
            }
        }

        if (totalApiDeleted > 0 || totalPortalDeleted > 0)
        {
            _logger.LogInformation("Deleted {ApiCount} ApiMethodDefinitions and {PortalCount} PortalPageDefinitions",
                totalApiDeleted, totalPortalDeleted);
        }
    }

    public async Task<(int modelsDeleted, int modulesDeleted)> DeleteModelsAndModulesAsync(
        List<string> modulesToDelete,
        List<(string Name, string ModuleName)> modelsToDelete)
    {
        int modelsDeleted = 0;
        int modulesDeleted = 0;
        var modulesToDeleteSet = modulesToDelete.ToHashSet();

        foreach (var (name, moduleName) in modelsToDelete)
        {
            if (modulesToDeleteSet.Contains(moduleName))
                continue;

            try
            {
                var modelDto = await _modelRepository.GetByPkAsync(name, moduleName);
                if (modelDto.Code == 0 && modelDto.Value != null)
                {
                    EnsureConnectionString(_writeContext);
                    var deleteResult = await _modelRepository.DeleteAsync(modelDto.Value);
                    if (deleteResult.Code == 0 && deleteResult.Value)
                        modelsDeleted++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error deleting Model {Name} in module {ModuleName}", name, moduleName);
            }
        }

        foreach (var moduleName in modulesToDelete)
        {
            try
            {
                EnsureConnectionString(_writeContext);
                var modelsResult = await _modelRepository.DeleteModelsWithModuleNameAsync(moduleName);
                if (modelsResult.Code == 0)
                    modelsDeleted += modelsResult.Value;

                var moduleDto = await _moduleRepository.GetByPkAsync(moduleName);
                if (moduleDto.Code == 0 && moduleDto.Value != null)
                {
                   EnsureConnectionString(_writeContext);
                    var deleteResult = await _moduleRepository.DeleteAsync(moduleDto.Value);
                    if (deleteResult.Code == 0 && deleteResult.Value)
                        modulesDeleted++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error deleting Module {ModuleName}", moduleName);
            }
        }

        if (modelsDeleted > 0 || modulesDeleted > 0)
        {
            _logger.LogInformation("Deleted {ModelCount} Models and {ModuleCount} Modules",
                modelsDeleted, modulesDeleted);
        }

        return (modelsDeleted, modulesDeleted);
    }
}