using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class ConditionTest
{
    private static string Path { get; } = "Metar/Models/TestData/Condition";

    public static TheoryData<string, Condition> TestCases => new()
    {
        {
            $"{Path}/Condition1.json",
            new Condition { Code = "OVC", Text = "Overcast" }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidConditionData_ReturnsValidObject(string expectedDataFilePath, Condition expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Condition>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Condition>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Code, actual: actualData?.Code);
        Assert.Equal(expected: expectedData.Text, actual: actualData?.Text);
    }
}