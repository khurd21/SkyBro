using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

using SkyBro.RequestHandler;

using Xunit;

namespace SkyBro.Tests.RequestHandler;

public class HelpIntentHandlerTest
{
    private HelpIntentHandler HandlerUnderTest { get; init; } = new();

    [Fact]
    public void CanHandle_ShouldReturnTrue_ForHelpIntentRequest()
    {
        // Arrange
        var intentRequest = new IntentRequest
        {
            Intent = new Intent
            {
                Name = "AMAZON.HelpIntent"
            }
        };
        var skillRequest = new SkillRequest
        {
            Request = intentRequest
        };

        // Act
        var result = HandlerUnderTest.CanHandle(skillRequest);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_ForNonIntentRequest()
    {
        // Arrange
        var request = new SessionEndedRequest();
        var skillRequest = new SkillRequest
        {
            Request = request
        };

        // Act
        var result = HandlerUnderTest.CanHandle(skillRequest);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_ForNonHelpIntentRequest()
    {
        // Arrange
        var intentRequest = new IntentRequest
        {
            Intent = new Intent
            {
                Name = "AMAZON.StopIntent"
            }
        };
        var skillRequest = new SkillRequest
        {
            Request = intentRequest
        };

        // Act
        var result = HandlerUnderTest.CanHandle(skillRequest);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Handle_ShouldReturnCorrectResponse_ForHelpIntentRequest()
    {
        // Arrange
        var intentRequest = new IntentRequest
        {
            Intent = new Intent
            {
                Name = "AMAZON.HelpIntent"
            }
        };
        var skillRequest = new SkillRequest
        {
            Request = intentRequest
        };

        // Act
        var response = HandlerUnderTest.Handle(skillRequest);

        // Assert
        var outputSpeech = response.Response.OutputSpeech as PlainTextOutputSpeech;
        Assert.NotNull(outputSpeech);
        Assert.Equal("Here to help! Ask me anything.", outputSpeech.Text);

        var reprompt = response.Response.Reprompt.OutputSpeech as PlainTextOutputSpeech;
        Assert.NotNull(reprompt);
        Assert.Equal("Ask for weather at Skydive Kapowsin.", reprompt.Text);
    }
}