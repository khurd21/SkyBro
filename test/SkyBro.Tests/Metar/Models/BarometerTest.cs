using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class BarometerTest
{
    private static string Path { get; } = "Metar/Models/TestData/Barometer";

    public static TheoryData<string, Barometer> TestCases => new()
    {
        {
            $"{Path}/Barometer1.json",
            new Barometer { Hg = 30.17M, Hpa = 1022.0M, Kpa = 102.17M, Mb = 1021.67M }
        },
        {
            $"{Path}/Barometer2.json",
            new Barometer { Hg = 30.12M, Hpa = 1020.0M, Kpa = 102.0M, Mb = 1019.98M }
        },
        {
            $"{Path}/Barometer3.json",
            new Barometer { Hg = -3.81M, Hpa = 42.0M, Kpa = 12.05M, Mb = -1019.98M }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidBarometricData_ReturnsValidObject(string expectedBarometerDataFilePath, Barometer expectedBarometerData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedBarometerDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Barometer>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Barometer>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedBarometerData.Hg, actual: actualData?.Hg);
        Assert.Equal(expected: expectedBarometerData.Hpa, actual: actualData?.Hpa);
        Assert.Equal(expected: expectedBarometerData.Kpa, actual: actualData?.Kpa);
        Assert.Equal(expected: expectedBarometerData.Mb, actual: actualData?.Mb);
    }
}