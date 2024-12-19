using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class CeilingTest
{
    private static string Path { get; } = "Metar/Models/TestData/Ceiling";

    public static TheoryData<string, Ceiling> TestCases => new()
    {
        {
            $"{Path}/Ceiling1.json",
            new Ceiling { Feet = 8500, Meters = 2591 }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidCeilingData_ReturnsValidObject(string expectedDataFilePath, Ceiling expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Ceiling>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Ceiling>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Feet, actual: actualData?.Feet);
        Assert.Equal(expected: expectedData.Meters, actual: actualData?.Meters);
    }
}