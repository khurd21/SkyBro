using Xunit;
using Amazon.Lambda.Core;
using Alexa.NET.Request;
using Amazon.Lambda.TestUtilities;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace WeatherObservations.Tests;

public class FunctionTest
{
    Function function { get; set; }

    TestLambdaContext lambdaContext { get; set; }

    public FunctionTest()
    {
        this.function = new();
        this.lambdaContext = new();
    }

    [Theory]
    [InlineData("KSHN:WA")]
    public void TestFunctionHandler(string id)
    {
        var intentRequest = new IntentRequest()
        {
            Intent = new()
            {
                Name = "WeatherObservationsIntent",
                Slots = new Dictionary<string, Slot>()
                {
                    {
                        "airport", new()
                        {
                            Resolution = new()
                            {
                                Authorities = new ResolutionAuthority[1]
                                {
                                    new()
                                    {
                                        Values = new ResolutionValueContainer[1]
                                        {
                                            new()
                                            {
                                                Value = new()
                                                {
                                                    Id = id,
                                                }
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    },
                    {
                        "date", new()
                        {
                            SlotValue = new()
                            {
                                Value = DateTime.Now.ToString(),
                            }
                        }
                    },
                }
            }
        };

        var skillRequest = new SkillRequest()
        {
            Request = intentRequest,
        };

        var response = this.function.FunctionHandler(skillRequest, this.lambdaContext);
    }
}
