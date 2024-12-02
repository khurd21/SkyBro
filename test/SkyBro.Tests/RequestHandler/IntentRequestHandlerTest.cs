using System.Security.Cryptography;

using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

using SkyBro.RequestHandler;

using Xunit;

namespace SkyBro.Tests.Requestandler;

public class IntentRequestHandlerTest
{
    private IntentRequestHandler IntentRequestHandlerUnderTest { get; init; }

    public IntentRequestHandlerTest()
    {
        IntentRequestHandlerUnderTest = new();
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_ForNonIntentRequest()
    {
        var skillRequest = new SkillRequest()
        {
            Request = new SessionEndedRequest(),
        };

        var result = IntentRequestHandlerUnderTest.CanHandle(skillRequest);

        Assert.False(result);
    }

    [Fact]
    public void CanHandle_ShouldReturnTrue_ForIntentRequest()
    {
        var skillRequest = new SkillRequest()
        {
            Request = new IntentRequest(),
        };

        var result = IntentRequestHandlerUnderTest.CanHandle(skillRequest);

        Assert.True(result);
    }

    [Fact]
    public void Handl_ShouldReturnErrorResponse_ForUnknownIntent()
    {
        var skillRequest = new SkillRequest()
        {
            Request = new IntentRequest()
            {
                Intent = new()
                {
                    Name = "Unknown Name",
                },
            },
        };

        var response = IntentRequestHandlerUnderTest.Handle(skillRequest);
        var outputSpeech = response.Response.OutputSpeech as PlainTextOutputSpeech;

        Assert.NotNull(outputSpeech);
        Assert.Equal("I'm not sure how to help with that.", outputSpeech.Text);
    }

    [Theory]
    [InlineData("AMAZON.HelpIntent", "Here to help! Ask me anything.")]
    [InlineData("AMAZON.CancelIntent", "Blue skies!")]
    [InlineData("WeatherObservationsIntent", "Weather observations.")]
    public void Handle_ShouldReturnHelpResponse_ForHelpIntent(string intentName, string expectedOutputSpeech)
    {
        var skillRequest = new SkillRequest()
        {
            Request = new IntentRequest()
            {
                Intent = new()
                {
                    Name = intentName,
                },
            },
        };

        var response = IntentRequestHandlerUnderTest.Handle(skillRequest);
        var outputSpeech = response.Response.OutputSpeech as PlainTextOutputSpeech;

        Assert.NotNull(outputSpeech);
        Assert.Equal(expectedOutputSpeech, outputSpeech.Text);
    }

    [Theory]
    [InlineData("AMAZON.HelpIntent", "Ask for weather at Skydive Kapowsin.")]
    public void Handle_ShouldReturnHelpReprompt_ForHelpIntent(string intentName, string expectedRepromptOutputSpeech)
    {
        var skillRequest = new SkillRequest()
        {
            Request = new IntentRequest()
            {
                Intent = new()
                {
                    Name = intentName,
                },
            },
        };

        var response = IntentRequestHandlerUnderTest.Handle(skillRequest);
        var repromptSpeech = response.Response.Reprompt.OutputSpeech as PlainTextOutputSpeech;
        Assert.NotNull(repromptSpeech);
        Assert.Equal(expectedRepromptOutputSpeech, repromptSpeech.Text);
    }
}