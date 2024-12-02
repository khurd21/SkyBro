using Alexa.NET.Response;

using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using Xunit;

namespace SkyBro.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var response = function.FunctionHandler(new(), context);

        var plainTextOutputSpeech = response.Response.OutputSpeech as PlainTextOutputSpeech;
        Assert.NotNull(plainTextOutputSpeech);
        Assert.Equal("I'm sorry, I can't handle that request.", plainTextOutputSpeech.Text);
    }
}