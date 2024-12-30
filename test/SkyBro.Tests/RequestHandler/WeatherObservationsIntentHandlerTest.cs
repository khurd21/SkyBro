using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

using Moq;

using Newtonsoft.Json;

using SkyBro.Metar;
using SkyBro.Metar.Models;
using SkyBro.RequestHandler;
using SkyBro.Services;

using Xunit;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SkyBro.Tests.RequestHandler;

public class WeatherObservationsIntentHandlerTest
{
    private WeatherObservationsIntentHandler HandlerUnderTest { get; init; }

    private Mock<IMetarEndpoint> MockWeatherClient { get; init; }

    private static string Path { get; } = "RequestHandler/TestData";

    public WeatherObservationsIntentHandlerTest()
    {
        MockWeatherClient = new();
        HandlerUnderTest = new(MockWeatherClient.Object);
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_ForIntentRequestWithWrongName()
    {
        // Arrange
        var intentRequest = new IntentRequest
        {
            Intent = new Intent
            {
                Name = "MyRandomIntent"
            }
        };
        var skillRequest = new SkillRequest
        {
            Request = intentRequest
        };

        // Act
        var actualResult = HandlerUnderTest.CanHandle(skillRequest);

        // Assert
        Assert.False(actualResult);
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_ForSkillRequestWithWrongRequestType()
    {
        // Arrange
        var skillRequest = new SkillRequest
        {
            Request = new LaunchRequest()
        };

        // Act
        var actualResult = HandlerUnderTest.CanHandle(skillRequest);

        // Assert
        Assert.False(actualResult);
    }

    [Fact]
    public void CanHandle_ShouldReturnTrue_ForIntentRequestWithCorrectName()
    {
        // Arrange
        var intentRequest = new IntentRequest
        {
            Intent = new Intent
            {
                Name = "WeatherObservationsIntent"
            }
        };
        var skillRequest = new SkillRequest
        {
            Request = intentRequest
        };

        // Act
        var actualResult = HandlerUnderTest.CanHandle(skillRequest);

        // Assert
        Assert.True(actualResult);
    }

    [Theory]
    [InlineData("MetarStationData-UNUSED.json", "SkillRequest-ER_SUCCESS_NO_MATCH.json", "ExpectedMessage-CouldNotProcess.txt")]
    [InlineData("MetarStationData-UNUSED.json", "SkillRequest-ER_SUCCESS_MATCH-WrongId.json", "ExpectedMessage-CouldNotProcess.txt")]
    public void Handle_Something(string stationDataFileName, string skillRequestFileName, string messageFileName)
    {
        // Arrange
        var stationDataFilePath = $"{Path}/{stationDataFileName}";
        var skillRequestFilePath = $"{Path}/{skillRequestFileName}";
        var messageFilePath = $"{Path}/{messageFileName}";

        var expectedStationData = JsonSerializer.Deserialize<MetarStationData>(File.ReadAllText(stationDataFilePath))!;
        var expectedSkillRequest = JsonConvert.DeserializeObject<SkillRequest>(File.ReadAllText(skillRequestFilePath))!;
        var expectedMessage = File.ReadAllText(messageFilePath);

        MockWeatherClient.Setup(client => client.GetNearestMetarAsync(It.IsAny<string>()))
            .ReturnsAsync(new APIResponse<MetarStationData>
            {
                Data = expectedStationData,
                Success = true,
                Message = string.Empty
            });

        MockWeatherClient.Setup(client => client.GetNearestMetarAsync(It.IsAny<SkyBro.Metar.Models.GeolocationCoordinate>()))
            .ReturnsAsync(new APIResponse<MetarStationData>
            {
                Data = expectedStationData,
                Success = true,
                Message = string.Empty
            });


        // Act
        var actualResponse = HandlerUnderTest.Handle(expectedSkillRequest);

        // Assert
        var actualResponseText = actualResponse.Response.OutputSpeech as PlainTextOutputSpeech;
        Assert.NotNull(actualResponseText);
        Assert.Equal(expected: expectedMessage, actualResponseText.Text);
    }
}