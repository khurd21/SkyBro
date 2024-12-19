using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class VisibilityTest
{
    private static string Path { get; } = "Metar/Models/TestData/Visibility";

    public static TheoryData<string, Visibility> TestCases => new()
    {
        {
            $"{Path}/Visibility1.json",
            new Visibility { Miles = "Greater than 10", Meters = "16,100", MilesFloat = 10.0F, MetersFloat = 16_100.0F }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidHumidityData_ReturnsValidObject(string expectedDataFilePath, Visibility expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Visibility>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Visibility>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Miles, actual: actualData?.Miles);
        Assert.Equal(expected: expectedData.MilesFloat, actual: actualData?.MilesFloat);
        Assert.Equal(expected: expectedData.Meters, actual: actualData?.Meters);
        Assert.Equal(expected: expectedData.MetersFloat, actual: actualData?.MetersFloat);
    }
}