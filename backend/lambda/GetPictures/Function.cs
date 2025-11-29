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
            _table = Table.LoadTable(client, "PicturesTable"); // must match your Terraform name
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(
            APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            var scan = await _table.Scan(new ScanOperationConfig()).GetNextSetAsync();

            var pictures = scan.Select(item => new Picture
            {
                Id = item["Id"],
                Url = item.ContainsKey("Url") ? item["Url"] : null,
                Description = item.ContainsKey("Description") ? item["Description"] : null
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
