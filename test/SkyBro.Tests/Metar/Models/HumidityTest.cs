using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class HumidityTest
{
    private static string Path { get; } = "Metar/Models/TestData/Humidity";

    public static TheoryData<string, Humidity> TestCases => new()
    {
        {
            $"{Path}/Humidity1.json",
            new Humidity { Percent = 56 }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidHumidityData_ReturnsValidObject(string expectedDataFilePath, Humidity expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Humidity>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Humidity>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Percent, actual: actualData?.Percent);
    }
}