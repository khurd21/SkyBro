using Amazon.Lambda;
using Amazon.Lambda.Model;
using Newtonsoft.Json.Linq;

namespace WeatherObservations.IntegrationTests.API;

public class LambdaAPI
{
    private AmazonLambdaClient LambdaClient { get; set; }

    private static string ServiceURL { get; } = "http://127.0.0.1:3001";

    private static string FunctionName = "WeatherObservations";

    public LambdaAPI()
    {
        this.LambdaClient = new(new AmazonLambdaConfig
        {
            ServiceURL = LambdaAPI.ServiceURL,
            UseHttp = true,
        });
    }

    public async Task<JObject> InvokeAndGetPayload(string payload)
    {
        InvokeRequest invokeRequest = new()
        {
            InvocationType = InvocationType.RequestResponse,
            FunctionName = LambdaAPI.FunctionName,
            Payload = payload
        };

        return JObject.Parse(new System.IO.StreamReader(
                (await this.LambdaClient.InvokeAsync(invokeRequest)).Payload)
                .ReadToEnd());
    }
}