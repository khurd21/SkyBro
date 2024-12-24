using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class CloudTest
{
    private static string Path { get; } = "Metar/Models/TestData/Cloud";

    public static TheoryData<string, Cloud> TestCases => new()
    {
        {
            $"{Path}/Cloud1.json",
            new Cloud { BaseFeetAGL = 8500, BaseMetersAGL = 2591, Code = "OVC", Text = "Overcast", Feet = 8500, Meters = 2591 }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidCloudData_ReturnsValidObject(string expectedDataFilePath, Cloud expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Cloud>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Cloud>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.BaseFeetAGL, actual: actualData?.BaseFeetAGL);
        Assert.Equal(expected: expectedData.BaseMetersAGL, actual: actualData?.BaseMetersAGL);
        Assert.Equal(expected: expectedData.Code, actual: actualData?.Code);
        Assert.Equal(expected: expectedData.Text, actual: actualData?.Text);
        Assert.Equal(expected: expectedData.Feet, actual: actualData?.Feet);
        Assert.Equal(expected: expectedData.Meters, actual: actualData?.Meters);
    }
}