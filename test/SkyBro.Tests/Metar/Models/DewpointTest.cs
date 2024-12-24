using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class DewpointTest
{
    private static string Path { get; } = "Metar/Models/TestData/Dewpoint";

    public static TheoryData<string, Dewpoint> TestCases => new()
    {
        {
            $"{Path}/Dewpoint1.json",
            new Dewpoint { Celsius = -7, Fahrenheit = 19 }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidDewpointData_ReturnsValidObject(string expectedDataFilePath, Dewpoint expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Dewpoint>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Dewpoint>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Celsius, actual: actualData?.Celsius);
        Assert.Equal(expected: expectedData.Fahrenheit, actual: actualData?.Fahrenheit);
    }
}