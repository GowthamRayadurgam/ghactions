using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;

namespace TestFunctionApp
{
    public class HelloFunction
    {
        [Function("GetData")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "data")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            var data = new
            {
                message = "Hello from Azure Function App!",
                timestamp = DateTime.UtcNow,
                version = "1.0",
                status = "success"
            };

            response.WriteString(JsonSerializer.Serialize(data));
            return response;
        }

        [Function("HealthCheck")]
        public HttpResponseData HealthCheck(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            var healthData = new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow
            };

            response.WriteString(JsonSerializer.Serialize(healthData));
            return response;
        }
    }
}
