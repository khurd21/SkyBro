using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;

using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using Moq;

using Ninject;

using SkyBro.RequestHandler;

using Xunit;

namespace SkyBro.Tests;

public class FunctionTest
{
    private Mock<ISkillRequestDispatcher> MockSkillRequestDispatcher { get; init; }

    private StandardKernel TestKernel { get; init; }

    public FunctionTest()
    {
        MockSkillRequestDispatcher = new();
        TestKernel = new();
        TestKernel.Bind<ISkillRequestDispatcher>().ToConstant(MockSkillRequestDispatcher.Object);
    }

    [Fact]
    public void TestFunction()
    {
        var function = new Function(TestKernel);
        var context = new TestLambdaContext();
        var skillRequest = new SkillRequest();
        var expectedResponse = ResponseBuilder.Tell("Hello, Test!");
        MockSkillRequestDispatcher.Setup(d => d.Dispatch(skillRequest)).Returns(expectedResponse);

        var actualResponse = function.FunctionHandler(skillRequest, context);

        MockSkillRequestDispatcher.Verify(d => d.Dispatch(skillRequest), Times.Once);
        Assert.Equal(expected: expectedResponse, actual: actualResponse);
    }
}