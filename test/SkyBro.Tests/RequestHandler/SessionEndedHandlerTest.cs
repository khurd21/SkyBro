using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

using SkyBro.RequestHandler;

using Xunit;

namespace SkyBro.Tests.RequestHandler;

public class SessionEndedHandlerTest
{
    private SessionEndedRequestHandler SessionEndedHandlerUnderTest { get; init; }

    public SessionEndedHandlerTest()
    {
        SessionEndedHandlerUnderTest = new();
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_ForNonSessionEndedRequest()
    {
        var skillRequest = new SkillRequest()
        {
            Request = new LaunchRequest(),
        };

        var result = SessionEndedHandlerUnderTest.CanHandle(skillRequest);

        Assert.False(result);
    }

    [Fact]
    public void CanHandle_ShouldReturnTrue_ForSessionEndedRequest()
    {
        var skillRequest = new SkillRequest()
        {
            Request = new SessionEndedRequest(),
        };

        var result = SessionEndedHandlerUnderTest.CanHandle(skillRequest);

        Assert.True(result);
    }

    [Fact]
    public void Handle_ShouldReturnSessionEndedResponse_ForSessionEndedRequest()
    {
        var skillRequest = new SkillRequest()
        {
            Request = new LaunchRequest(),
        };

        var response = SessionEndedHandlerUnderTest.Handle(skillRequest);
        var outputSpeech = response.Response.OutputSpeech as PlainTextOutputSpeech;

        Assert.NotNull(outputSpeech);
        Assert.Equal("Blue skies!", outputSpeech.Text);
    }
}