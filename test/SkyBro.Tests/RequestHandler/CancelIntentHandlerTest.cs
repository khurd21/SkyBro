using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

using SkyBro.RequestHandler;

using Xunit;

namespace SkyBro.Tests.RequestHandler;

public class CancelIntentHandlerTest
{
    private CancelIntentHandler HandlerUnderTest { get; init; } = new();

    [Fact]
    public void CanHandle_CancelIntent_ReturnsTrue()
    {
        // Arrange
        var request = new SkillRequest
        {
            Request = new IntentRequest
            {
                Intent = new Intent
                {
                    Name = "AMAZON.CancelIntent"
                }
            }
        };

        // Act
        var result = HandlerUnderTest.CanHandle(request);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanHandle_NonCancelIntent_ReturnsFalse()
    {
        // Arrange
        var request = new SkillRequest
        {
            Request = new IntentRequest
            {
                Intent = new Intent
                {
                    Name = "AMAZON.HelpIntent"
                }
            }
        };

        // Act
        var result = HandlerUnderTest.CanHandle(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanHandle_NonIntentRequest_ReturnsFalse()
    {
        // Arrange
        var request = new SkillRequest
        {
            Request = new LaunchRequest()
        };

        // Act
        var result = HandlerUnderTest.CanHandle(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Handle_CancelIntent_ReturnsCorrectResponse()
    {
        // Arrange
        var request = new SkillRequest
        {
            Request = new IntentRequest
            {
                Intent = new Intent
                {
                    Name = "AMAZON.CancelIntent"
                }
            }
        };

        // Act
        var response = HandlerUnderTest.Handle(request);

        // Assert
        var outputSpeech = response.Response.OutputSpeech as SsmlOutputSpeech;
        Assert.NotNull(outputSpeech);
        Assert.Equal(
            @"<speak>
                <amazon:emotion name='excited' intensity='low'>
                    Blue skies!
                </amazon:emotion>
            </speak>", outputSpeech.Ssml);
    }
}