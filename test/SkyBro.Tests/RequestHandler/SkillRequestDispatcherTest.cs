using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

using Moq;

using SkyBro.RequestHandler;

using Xunit;

namespace SkyBro.Tests.RequestHandler;

public class SkillRequestDispatcherTest
{
    private SkillRequestDispatcher SkillRequestDispatcherUnderTest { get; init; }

    private Mock<ISkillRequestHandler> MockSkillRequest { get; init; }

    public SkillRequestDispatcherTest()
    {
        MockSkillRequest = new();
        SkillRequestDispatcherUnderTest = new(new List<ISkillRequestHandler>
        {
            MockSkillRequest.Object
        });
    }

    [Fact]
    public void Dispatch_ShouldReturnErrorResponse_ForLaunchRequest()
    {
        MockSkillRequest.Setup(h => h.CanHandle(It.IsAny<SkillRequest>())).Returns(false);

        var request = new SkillRequest()
        {
            Request = new LaunchRequest(),
        };

        var response = SkillRequestDispatcherUnderTest.Dispatch(request);
        var outputSpeech = response.Response.OutputSpeech as PlainTextOutputSpeech;

        MockSkillRequest.Verify(h => h.CanHandle(request), Times.Once);
        MockSkillRequest.Verify(h => h.Handle(request), Times.Never);

        Assert.NotNull(outputSpeech);
        Assert.Equal("I'm sorry, I'm not sure how to help with that.", outputSpeech.Text);
    }

    [Fact]
    public void Dispatch_ShouldReturnDefaultResponse_ForLaunchRequest()
    {
        MockSkillRequest.Setup(h => h.CanHandle(It.IsAny<SkillRequest>())).Returns(true);
        MockSkillRequest.Setup(h => h.Handle(It.IsAny<SkillRequest>())).Returns(ResponseBuilder.Tell("Default response."));

        var request = new SkillRequest()
        {
            Request = new LaunchRequest(),
        };

        var response = SkillRequestDispatcherUnderTest.Dispatch(request);
        var outputSpeech = response.Response.OutputSpeech as PlainTextOutputSpeech;

        MockSkillRequest.Verify(h => h.CanHandle(request), Times.Once);
        MockSkillRequest.Verify(h => h.Handle(request), Times.Once);

        Assert.NotNull(outputSpeech);
        Assert.Equal("Default response.", outputSpeech.Text);
    }
}