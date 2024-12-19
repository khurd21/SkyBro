using System.Text.Json;

using Alexa.NET.Request;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class GeolocationCoordinateConverterTest
{
    private GeolocationCoordinateConverter GeolocationCoordinateConverterUnderTest { get; init; } = new();

    private const string Path = "Metar/Models/TestData/GeolocationCoordinate";

    [Theory]
    [InlineData($"{Path}/ExtraData.json", "Unexpected data in coordinates array.", typeof(JsonException))]
    [InlineData($"{Path}/InvalidStart.json", "Coordinates must be an array.", typeof(JsonException))]
    [InlineData($"{Path}/MissingLatitude.json", "Expected latitude.", typeof(JsonException))]
    [InlineData($"{Path}/MissingLongitude.json", "Expected longitude.", typeof(JsonException))]
    public void Read_ShouldThrowException_ForInvalidInput(string filePath, string expectedErrorMessage, Type expectedExceptionType)
    {
        // Arrange
        var json = File.ReadAllText(filePath);
        var executeRead = (string json) =>
        {
            var reader = new Utf8JsonReader(new ReadOnlySpan<byte>(System.Text.Encoding.UTF8.GetBytes(json)));
            reader.Read();
            GeolocationCoordinateConverterUnderTest.Read(ref reader, typeof(GeolocationCoordinate), new());
        };

        // Act
        var actualException = Assert.Throws(exceptionType: expectedExceptionType, () => executeRead(json));

        // Assert
        Assert.Equal(expected: expectedErrorMessage, actual: actualException.Message);
    }

    [Fact]
    public void Read_ShouldReturnGeolocationCoordinate_ForValidInput()
    {
        // Arrange
        var json = File.ReadAllText($"{Path}/ValidCoordinate.json");
        var reader = new Utf8JsonReader(new ReadOnlySpan<byte>(System.Text.Encoding.UTF8.GetBytes(json)));
        var expectedCoordinates = new GeolocationCoordinate
        {
            Longitude = -73.779317M,
            Latitude = 40.639447M
        };
        reader.Read();

        // Act
        var actualCoordinates = GeolocationCoordinateConverterUnderTest.Read(ref reader, typeof(GeolocationCoordinate), new());

        // Assert
        Assert.NotNull(actualCoordinates);
        Assert.Equal(expected: expectedCoordinates.Latitude, actual: actualCoordinates.Latitude);
        Assert.Equal(expected: expectedCoordinates.Longitude, actual: actualCoordinates.Longitude);
    }

    [Fact]
    public void Write_ShouldThrowException_ForEmptyCoordinates()
    {
        // Arrange
        GeolocationCoordinate? emptyCoordinate = null;
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        // Act
        var actualException = Assert.Throws<JsonException>(() => GeolocationCoordinateConverterUnderTest.Write(writer, emptyCoordinate!, new()));
        Assert.Equal("Coordinates cannot be empty.", actualException.Message);
    }
}