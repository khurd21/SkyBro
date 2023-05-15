using Amazon.Lambda;

namespace WeatherObservations.IntegrationTests;

public static class Lambda
{
    public static AmazonLambdaClient LambdaClient { get; set; }

    static Lambda()
    {
        LambdaClient = new(new AmazonLambdaConfig
        {
            ServiceURL = "http://127.0.0.1:3001",
            UseHttp = true,
        });
    }
}