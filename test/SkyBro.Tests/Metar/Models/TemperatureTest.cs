using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class TemperatureTest
{
    private static string Path { get; } = "Metar/Models/TestData/Temperature";

    public static TheoryData<string, Temperature> TestCases => new()
    {
        {
            $"{Path}/Temperature1.json",
            new Temperature { Celsius = 1, Fahrenheit = 34 }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidHumidityData_ReturnsValidObject(string expectedDataFilePath, Temperature expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Temperature>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Temperature>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Celsius, actual: actualData?.Celsius);
        Assert.Equal(expected: expectedData.Fahrenheit, actual: actualData?.Fahrenheit);
    }
}