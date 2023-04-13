using Xunit;
using Moq;
using WeatherObservations.Dependencies.Logger;
using WeatherObservations.IntentHandlers.Amazon;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace WeatherObservations.Tests.IntentHandlers.Amazon;

public class AmazonIntentHandlerTests
{
    private Mock<ILogger> LoggerMock { get; set; }

    private AmazonIntentHandler AmazonIntentHandler { get; set; }

    public AmazonIntentHandlerTests()
    {
        this.LoggerMock = new();
        this.LoggerMock.Setup(l => l.Log(It.IsAny<string>()));

        this.AmazonIntentHandler = new(this.LoggerMock.Object);
    }
    
    [Fact]
    public void TestIntentName()
    {
        Assert.Equal("AMAZON", this.AmazonIntentHandler.IntentName);
    }

    [Theory]
    [InlineData("AMAZON.CancelIntent", "Blue skies")]
    [InlineData("AMAZON.StopIntent", "Blue skies")]
    [InlineData("AMAZON.HelpIntent", "You can ask me for sky conditions at an airport")]
    [InlineData("AMAZON.GarbageIntent", "You can ask me for sky conditions at an airport")]
    public void TestHandleIntentAsync_ReturnsOutputSpeech(string intentName, string outputSpeech)
    {
        SkillRequest request = new SkillRequestBuilder()
            .WithIntent(intentName)
            .Build();

        var response = this.AmazonIntentHandler.HandleIntentAsync((request.Request as IntentRequest)!).Result;

        Assert.NotNull(response);
        Assert.NotNull(response.Response);
        Assert.NotNull(response.Response.OutputSpeech);

        Assert.Contains(outputSpeech.ToLower(), (response.Response.OutputSpeech as PlainTextOutputSpeech)!.Text.ToLower());
    }

    [Theory]
    [InlineData("AMAZON.CancelIntent", "Blue skies")]
    public void TestHandleCancelIntentAsync(string intentName, string outputSpeech)
    {
        SkillRequest request = new SkillRequestBuilder()
            .WithIntent(intentName)
            .Build();
        
        var response = this.AmazonIntentHandler.HandleIntentAsync((request.Request as IntentRequest)!).Result;

        Assert.Contains(outputSpeech.ToLower(), (response.Response.OutputSpeech as PlainTextOutputSpeech)!.Text.ToLower());
    }

    [Theory]
    [InlineData("AMAZON.StopIntent", "Blue skies")]
    public void TestHandleStopIntentAsync(string intentName, string outputSpeech)
    {
        SkillRequest request = new SkillRequestBuilder()
            .WithIntent(intentName)
            .Build();

        var response = this.AmazonIntentHandler.HandleIntentAsync((request.Request as IntentRequest)!).Result;

        Assert.Contains(outputSpeech.ToLower(), (response.Response.OutputSpeech as PlainTextOutputSpeech)!.Text.ToLower());
    }

    [Theory]
    [InlineData("AMAZON.HelpIntent", "You can ask me for sky conditions at an airport")]
    public void TestHandleHelpIntentAsync(string intentName, string outputSpeech)
    {
        SkillRequest request = new SkillRequestBuilder()
            .WithIntent(intentName)
            .Build();

        var response = this.AmazonIntentHandler.HandleIntentAsync((request.Request as IntentRequest)!).Result;

        Assert.Contains(outputSpeech.ToLower(), (response.Response.OutputSpeech as PlainTextOutputSpeech)!.Text.ToLower());
    }

    [Theory]
    [InlineData("AMAZON.CancelIntent")]
    [InlineData("AMAZON.StopIntent")]
    [InlineData("AMAZON.HelpIntent")]
    public void TestHandleIntentAsync_LogsIntentName(string intentName)
    {
        SkillRequest request = new SkillRequestBuilder()
            .WithIntent(intentName)
            .Build();

        var response = this.AmazonIntentHandler.HandleIntentAsync((request.Request as IntentRequest)!).Result;

        this.LoggerMock.Verify(l => l.Log($"Handling intent: {intentName}."));
    }

    [Theory]
    [InlineData("GarbageIntent")]
    [InlineData("AMAZON.GarbageIntent")]
    public void TestHandleIntentAsync_LogsUnhandledIntent(string intentName)
    {
        SkillRequest request = new SkillRequestBuilder()
            .WithIntent(intentName)
            .Build();

        var response = this.AmazonIntentHandler.HandleIntentAsync((request.Request as IntentRequest)!).Result;

        this.LoggerMock.Verify(l => l.Log($"Unhandled intent: {intentName}"));
    }
}