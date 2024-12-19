using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class ElevationTest
{
    private static string Path { get; } = "Metar/Models/TestData/Elevation";

    public static TheoryData<string, Elevation> TestCases => new()
    {
        {
            $"{Path}/Elevation1.json",
            new Elevation { Feet = 10.0M, Meters = 3.0M }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidElevationData_ReturnsValidObject(string expectedDataFilePath, Elevation expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Elevation>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Elevation>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Feet, actual: actualData?.Feet);
        Assert.Equal(expected: expectedData.Meters, actual: actualData?.Meters);
    }
}