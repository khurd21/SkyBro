using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class WindTest
{
    private static string Path { get; } = "Metar/Models/TestData/Wind";

    public static TheoryData<string, Wind> TestCases => new()
    {
        {
            $"{Path}/Wind1.json",
            new Wind { Degrees = 280, SpeedKph = 22, SpeedKts = 12, SpeedMph = 14, SpeedMps = 6 }
        },
        {
            $"{Path}/Wind2.json",
            new Wind { Degrees = 270, SpeedKph = 13, SpeedKts = 7, SpeedMph = 8, SpeedMps = 4, GustKph = 26, GustKts = 14, GustMph = 16, GustMps = 7 }
        }
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidHumidityData_ReturnsValidObject(string expectedDataFilePath, Wind expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Wind>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Wind>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Degrees, actual: actualData?.Degrees);
        Assert.Equal(expected: expectedData.GustKph, actual: actualData?.GustKph);
        Assert.Equal(expected: expectedData.GustKts, actual: actualData?.GustKts);
        Assert.Equal(expected: expectedData.GustMph, actual: actualData?.GustMph);
        Assert.Equal(expected: expectedData.GustMps, actual: actualData?.GustMps);
        Assert.Equal(expected: expectedData.SpeedKph, actual: actualData?.SpeedKph);
        Assert.Equal(expected: expectedData.SpeedKts, actual: actualData?.SpeedKts);
        Assert.Equal(expected: expectedData.SpeedMph, actual: actualData?.SpeedMph);
        Assert.Equal(expected: expectedData.SpeedMps, actual: actualData?.SpeedMps);
    }
}