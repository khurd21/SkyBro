using System.Security.Cryptography;
using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class GeometryTest
{
    private static string Path { get; } = "Metar/Models/TestData/Geometry";

    public static TheoryData<string, Geometry> TestCases => new()
    {
        {
            $"{Path}/Geometry1.json",
            new Geometry { Coordinates = new GeolocationCoordinate { Longitude = -74.009003M, Latitude = 40.701199M }, Type = "Point" }
        }
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidGeometryData_ReturnsValidObject(string expectedDataFilePath, Geometry expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Geometry>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Geometry>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Coordinates.Latitude, actual: actualData?.Coordinates.Latitude);
        Assert.Equal(expected: expectedData.Coordinates.Longitude, actual: actualData?.Coordinates.Longitude);
        Assert.Equal(expected: expectedData.Type, actual: actualData?.Type);
    }
}