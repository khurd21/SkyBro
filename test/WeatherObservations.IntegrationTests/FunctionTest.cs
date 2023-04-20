using Amazon.Lambda;
using Amazon.Lambda.Model;
using Xunit;

namespace WeatherObservations.IntegrationTests;

public class FunctionTest
{
    [Fact]
    public void TestCancelIntent()
    {
        var client = Lambda.LambdaClient;

        var invokeRequest = new InvokeRequest()
        {
            InvocationType = InvocationType.RequestResponse,
            FunctionName = "WeatherObservations",
            Payload = File.ReadAllText("Payloads/HelpIntent.json")
        };

        var response = client.InvokeAsync(invokeRequest).Result;
        var responseString = new System.IO.StreamReader(response.Payload).ReadToEnd();
        Assert.NotNull(responseString);
    }
}