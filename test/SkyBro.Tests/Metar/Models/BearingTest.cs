using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class BearingTest
{
    private static string Path { get; } = "Metar/Models/TestData/Bearing";

    public static TheoryData<string, Bearing> TestCases => new()
    {
        {
            $"{Path}/Bearing1.json",
            new Bearing { From = 218, To = 38 }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidBearingData_ReturnsValidObject(string expectedBearingDataFilePath, Bearing expectedBearingData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedBearingDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Bearing>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Bearing>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedBearingData.From, actual: actualData?.From);
        Assert.Equal(expected: expectedBearingData.To, actual: actualData?.To);
    }
}