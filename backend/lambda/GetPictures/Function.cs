using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using GetPictures.models;
using StackExchange.Redis;

namespace GetPictures
{
    public class Function
    {
        private readonly Table _table;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _cache;

        public Function()
        {
            var client = new AmazonDynamoDBClient();
            _table = Table.LoadTable(client, "PicturesTable");

            var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
            _redis = ConnectionMultiplexer.Connect($"{redisHost}:6379");
            _cache = _redis.GetDatabase();
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(
            APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            var cached = await _cache.StringGetAsync("pictures");
            if (!cached.IsNullOrEmpty)
                return Response(cached);

            var scan = await _table.Scan(new ScanOperationConfig()).GetNextSetAsync();

            var pictures = scan.Select(item => new Picture
            {
                Id = item.TryGetValue("Id", out var id) ? id.AsString() : "",
                Url = item.TryGetValue("Url", out var url) ? url.AsString() : "",
                Description = item.TryGetValue("Description", out var desc) ? desc.AsString() : ""
            }).ToList();

            var json = JsonSerializer.Serialize(pictures);
            await _cache.StringSetAsync("pictures", json, TimeSpan.FromMinutes(10));
            return Response(json);
        }

        private APIGatewayHttpApiV2ProxyResponse Response(string json)
        {
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 200,
                Headers = new()
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                },
                Body = json
            };
        }
    }
}
