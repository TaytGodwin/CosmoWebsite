using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using GetPictures.models;
using StackExchange.Redis;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetPictures
{
    public class Function
    {
        private static readonly AmazonDynamoDBClient _dynamoClient = new AmazonDynamoDBClient();
        private static Table? _table;
        private static ConnectionMultiplexer? _redis;
        private static IDatabase? _cache;

        public Function()
        {
            _table ??= Table.LoadTable(_dynamoClient, "PicturesTable");

            if (_redis == null || !_redis.IsConnected)
            {
                var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
                if (!string.IsNullOrEmpty(redisHost))
                {
                    _redis = ConnectionMultiplexer.Connect($"{redisHost}:6379");
                    _cache = _redis.GetDatabase();
                }
            }
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(
            APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            try
            {
                // Try cache first
                if (_cache != null)
                {
                    var cached = await _cache.StringGetAsync("pictures");
                    if (!cached.IsNullOrEmpty)
                    {
                        context.Logger.LogLine("Returning cached data");
                        return Response(cached!);
                    }
                }

                context.Logger.LogLine("Cache miss, querying DynamoDB...");

                var scan = await _table!.Scan(new ScanOperationConfig()).GetNextSetAsync();

                var pictures = scan.Select(item => new Picture
                {
                    Id = item.TryGetValue("Id", out var id) ? id.AsString() : "",
                    Url = item.TryGetValue("Url", out var url) ? url.AsString() : "",
                    Description = item.TryGetValue("Description", out var desc) ? desc.AsString() : ""
                }).ToList();

                var json = JsonSerializer.Serialize(pictures);

                // Cache the result
                if (_cache != null)
                {
                    await _cache.StringSetAsync("pictures", json, TimeSpan.FromMinutes(10));
                    context.Logger.LogLine("Data cached for 10 minutes");
                }

                return Response(json);
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error: {ex.Message}");
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 500,
                    Body = JsonSerializer.Serialize(new { error = ex.Message })
                };
            }
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
}