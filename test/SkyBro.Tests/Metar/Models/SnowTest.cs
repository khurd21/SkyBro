using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class SnowTest
{
    private static string Path { get; } = "Metar/Models/TestData/Snow";

    public static TheoryData<string, Snow> TestCases => new()
    {
        {
            $"{Path}/Snow1.json",
            new Snow { Inches = 1, Millimeters = 25 }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidHumidityData_ReturnsValidObject(string expectedDataFilePath, Snow expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Snow>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Snow>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Inches, actual: actualData?.Inches);
        Assert.Equal(expected: expectedData.Millimeters, actual: actualData?.Millimeters);
    }
}