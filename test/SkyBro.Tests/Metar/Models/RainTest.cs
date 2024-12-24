using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class RainTest
{
    private static string Path { get; } = "Metar/Models/TestData/Rain";

    public static TheoryData<string, Rain> TestCases => new()
    {
        {
            $"{Path}/Rain1.json",
            new Rain { Inches = 0.04M, Millimeters = 1.02M }
        },
        {
            $"{Path}/Rain2.json",
            new Rain { Inches = 18, Millimeters = 457 }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidHumidityData_ReturnsValidObject(string expectedDataFilePath, Rain expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Rain>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Rain>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Inches, actual: actualData?.Inches);
        Assert.Equal(expected: expectedData.Millimeters, actual: actualData?.Millimeters);
    }
}