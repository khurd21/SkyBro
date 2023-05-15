using Amazon.Lambda.Model;
using Newtonsoft.Json.Linq;
using WeatherObservations.IntegrationTests.API;

namespace WeatherObservations.IntegrationTests.Steps;

public class StepStateHandler
{
    public JObject OutputPayload { get; private set; }

    public bool IsRequestComplete { get; private set; }

    private LambdaAPI Lambda { get; set; }

    private string InputPayload { get; set; }

    private static string BasePayloadDirectory { get; } = "Payloads";

    private static Dictionary<string, string> RequestToPayload { get; } = new()
    {
        { "help", "HelpIntent.json" },
        { "launch", "LaunchRequest.json" },
        { "session end", "SessionEndedRequest.json" },
        { "weather", "WeatherIntent.json" },
    };

    public StepStateHandler()
    {
        this.Lambda = new();
        this.InputPayload = string.Empty;
        this.OutputPayload = new();
        this.IsRequestComplete = false;
    }

    public void Initialize()
    {
        this.Lambda = new();
        this.InputPayload = string.Empty;
        this.OutputPayload = new();
        this.IsRequestComplete = false;
    }

    public void SetRequestType(string requestType)
    {
        if (!StepStateHandler.RequestToPayload.ContainsKey(requestType))
        {
            string error = $"Request type {requestType} not found.\n";
            error += "Available request types are:\n";
            foreach (string key in StepStateHandler.RequestToPayload.Keys)
            {
                error += $"  {key}\n";
            }
            throw new Exception(error);
        }

        string payload = StepStateHandler.RequestToPayload[requestType];
        string payloadPath = System.IO.Path.Combine(StepStateHandler.BasePayloadDirectory, payload);
        this.InputPayload = File.ReadAllText(payloadPath);
    }

    public void SendRequest()
    {
        this.OutputPayload = this.Lambda
            .InvokeAndGetPayload(this.InputPayload)
            .Result;
        this.IsRequestComplete = true;
    }
}