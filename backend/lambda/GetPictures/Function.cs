using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using GetPictures.models;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetPictures
{
    public class Function
    {
        private readonly Table _table;

        public Function()
        {
            var client = new AmazonDynamoDBClient();
            _table = Table.LoadTable(client, "PicturesTable");
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(
            APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            // Scan all items in table
            var items = await _table.Scan(new ScanOperationConfig()).GetNextSetAsync();

            // Safe mapping to avoid null exceptions
            var pictures = items.Select(item => new Picture
            {
                Id = item.TryGetValue("Id", out var id) ? id.AsString() : "",
                Url = item.TryGetValue("Url", out var url) ? url.AsString() : "",
                Description = item.TryGetValue("Description", out var desc)
                    ? desc.AsString()
                    : ""
            }).ToList();

            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 200,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"},
                    {"Access-Control-Allow-Origin", "*"}
                },
                Body = JsonSerializer.Serialize(pictures)
            };
        }
    }
}
