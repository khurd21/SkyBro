using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

using SkyBro.RequestHandler;

using Xunit;

namespace SkyBro.Tests.RequestHandler;

public class LaunchRequesthandlerTest
{
    private LaunchRequestHandler LaunchRequestHandlerUnderTest { get; init; }

    public LaunchRequesthandlerTest()
    {
        LaunchRequestHandlerUnderTest = new();
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_ForNonLaunchRequest()
    {
        var skillRequest = new SkillRequest()
        {
            Request = new SessionEndedRequest(),
        };

        var result = LaunchRequestHandlerUnderTest.CanHandle(skillRequest);

        Assert.False(result);
    }

    [Fact]
    public void CanHandle_ShouldReturnTrue_ForLaunchRequest()
    {
        var skillRequest = new SkillRequest()
        {
            Request = new LaunchRequest(),
        };

        var result = LaunchRequestHandlerUnderTest.CanHandle(skillRequest);

        Assert.True(result);
    }

    [Fact]
    public void Handle_ShouldReturnLaunchResponseWithReprompt_ForLaunchRequest()
    {
        var skillRequest = new SkillRequest()
        {
            Request = new LaunchRequest(),
        };

        var response = LaunchRequestHandlerUnderTest.Handle(skillRequest);
        var outputSpeech = response.Response.OutputSpeech as PlainTextOutputSpeech;
        var repromptSpeech = response.Response.Reprompt.OutputSpeech as PlainTextOutputSpeech;

        Assert.NotNull(outputSpeech);
        Assert.NotNull(repromptSpeech);
        Assert.Equal("Welcome to SkyBro! To start, ask me for the weather at a dropzone of your choice.", outputSpeech.Text);
        Assert.Equal("Ask me for the weather at a dropzone. For example, say 'what is the weather at Skydive Kapowsin?'.", repromptSpeech.Text);
    }
}