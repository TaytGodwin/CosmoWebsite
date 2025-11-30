using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using GetPictures.models;
using StackExchange.Redis;

public class Function
{
    private readonly Table _table;
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _cache;

    public Function()
    {
        var client = new AmazonDynamoDBClient();
        _table = Table.LoadTable(client, "PicturesTable");

        // Connect to Redis
        var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
        _redis = ConnectionMultiplexer.Connect($"{redisHost}:6379");
        _cache = _redis.GetDatabase();
    }

    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(
        APIGatewayHttpApiV2ProxyRequest request,
        ILambdaContext context)
    {
        // 1. Check Redis cache first
        var cached = await _cache.StringGetAsync("pictures");
        if (!cached.IsNullOrEmpty)
        {
            return Response(cached);
        }

        // 2. Load from DynamoDB
        var scan = await _table.Scan(new ScanOperationConfig()).GetNextSetAsync();

        var pictures = scan.Select(item => new Picture
        {
            Id = item.TryGetValue("Id", out var id) ? id.AsString() : "",
            Url = item.TryGetValue("Url", out var url) ? url.AsString() : "",
            Description = item.TryGetValue("Description", out var desc) ? desc.AsString() : ""
        }).ToList();

        var json = JsonSerializer.Serialize(pictures);

        // 3. Store in Redis for 10 minutes
        await _cache.StringSetAsync("pictures", json, TimeSpan.FromMinutes(10));

        return Response(json);
    }

    private APIGatewayHttpApiV2ProxyResponse Response(string json)
    {
        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" }
            },
            Body = json
        };
    }
}
