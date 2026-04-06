using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickCode.MyecommerceDemo.Common.Extensions;
using QuickCode.MyecommerceDemo.Common.Nswag;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.Common.Nswag.Clients.IdentityModuleApi.Contracts;
using QuickCode.MyecommerceDemo.Gateway.HTTP;
using QuickCode.MyecommerceDemo.Gateway.KafkaProducer;

namespace QuickCode.MyecommerceDemo.Gateway.Extensions;
public static class KafkaHelper
{
    internal static async Task<bool> GetTokenIsValid(IServiceProvider services, string token)
    {
        var authenticationsClient = services.GetRequiredService<IAuthenticationsClient>();
        return !token.IsTokenExpired() && await authenticationsClient.ApiAuthValidatePostAsync(token);
    }

    static async Task<List<GetKafkaEventsResponseDto>> GetKafkaEvents(IServiceProvider services, IMemoryCache memoryCache)
    {
        var configuration = services.GetRequiredService<IConfiguration>();
        var kafkaEventsClient = services.GetRequiredService<IKafkaEventsClient>();

        return await memoryCache.GetOrAddAsync("KafkaEvents",
            async _ => await GetAllKafkaEvents(configuration, kafkaEventsClient),
            TimeSpan.FromMinutes(1));
    }

    static void SetKafkaApiKeyToClients(IKafkaEventsClient kafkaEventsClient, string configIdentityApiKey)
    {
        (kafkaEventsClient as ClientBase)?.SetApiKey(configIdentityApiKey);
    }

    static async Task<List<GetKafkaEventsResponseDto>> GetAllKafkaEvents(IConfiguration configuration,
        IKafkaEventsClient kafkaEventsClient)
    {
        var configApiKey = configuration.GetValue<string>("QuickcodeApiKeys:IdentityModuleApiKey");
        SetKafkaApiKeyToClients(kafkaEventsClient, configApiKey!);
        return (await kafkaEventsClient.KafkaEventsGetKafkaEventsAsync()).ToList();
    }

    internal static async Task<GetKafkaEventsResponseDto?> CheckKafkaEventExists(IServiceProvider services,
        IMemoryCache memoryCache, HttpContext context)
    {
        var allKafkaEvents = await GetKafkaEvents(services, memoryCache);
        var kafkaEvent = allKafkaEvents.Find(endpoint =>
            endpoint.UrlPath.IsRouteMatch(context.Request.Path) &&
            endpoint.IsActive &&
            endpoint.HttpMethod.ToString().Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase));

        if (kafkaEvent == null)
        {
            return null;
        }

        kafkaEvent = GetKafkaEventsResponseDto.FromJson(kafkaEvent.ToJson());
        kafkaEvent.TopicName = $"{kafkaEvent.TopicName}_{kafkaEvent.HttpMethod.ToString().ToLowerInvariant()}";
        return kafkaEvent;
    }

    private static JObject ConvertToJsonObject(string json)
    {
        try
        {
            var body = JToken.Parse(json);
            return body is JObject jsonObject ? jsonObject : new JObject { ["return"] = body };
        }
        catch (JsonReaderException)
        {
            return new JObject { ["error"] = "Invalid JSON format", ["value"] = json };
        }
    }

    internal static async Task SendKafkaMessageIfEventExists(IServiceProvider services, IMemoryCache memoryCache,
        IKafkaProducerWrapper kafkaProducer, HttpContext context, Stopwatch stopwatch)
    {
        var kafkaEvent = await CheckKafkaEventExists(services, memoryCache, context);
        if (kafkaEvent is null) return;

        var requestBodyText = await context.TryGetRequestBodyAsync();

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body, leaveOpen: true);
        var responseBodyText = await reader.ReadToEndAsync();

        var kafkaMessage = new KafkaMessage
        {
            RequestInfo = new RequestInfo
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                Headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                Body = ConvertToJsonObject(requestBodyText)
            },
            ResponseInfo = new ResponseInfo
            {
                StatusCode = context.Response.HasStarted ? context.Response.StatusCode : 500,
                Headers = context.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                Body = ConvertToJsonObject(responseBodyText)
            },
            ElapsedMilliseconds = (int)stopwatch.ElapsedMilliseconds,
            Timestamp = DateTime.UtcNow
        };

        var key = GenerateKey(context);
        await SendKafkaMessage(kafkaProducer, kafkaEvent.TopicName, key, kafkaMessage);
    }

    internal static async Task SendErrorKafkaMessage(IKafkaProducerWrapper kafkaProducer, string topic,
        HttpContext context, Stopwatch stopwatch, Exception ex)
    {
        var errorKafkaMessage = new KafkaMessage
        {
            RequestInfo = new RequestInfo
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                Headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
            },
            ResponseInfo = new ResponseInfo
            {
                StatusCode = context.Response.HasStarted ? context.Response.StatusCode : 500,
                Body = new JObject { ["message"] = "An error occurred." }
            },
            ExceptionMessage = ex.Message,
            ElapsedMilliseconds = (int)stopwatch.ElapsedMilliseconds,
            Timestamp = DateTime.UtcNow
        };

        await SendKafkaMessage(kafkaProducer, topic, GenerateKey(context), errorKafkaMessage);
    }

    static string GenerateKey(HttpContext context) =>
        $"{context.Request.Method}|{context.Request.Path}|{DateTime.UtcNow.Ticks}";

    private static async Task SendKafkaMessage(IKafkaProducerWrapper kafkaProducer, string topic, string key, KafkaMessage message)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var serializedMessage = JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include
            });
            
            await kafkaProducer.ProduceAsync(topic, key, serializedMessage);
            Console.WriteLine($"Message sent to Kafka topic: {topic}, Key: {key}");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"Message sending to Kafka timed out. Topic: {topic}, Key: {key}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send message to Kafka. Topic: {topic}, Key: {key}, Error: {ex.Message}");
        }
    }
}