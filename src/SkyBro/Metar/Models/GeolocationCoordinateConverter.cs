using System.Text.Json;
using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class GeolocationCoordinateConverter : JsonConverter<GeolocationCoordinate>
{
    public override GeolocationCoordinate? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Coordinates must be an array.");
        }

        reader.Read();
        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException("Expected longitude.");
        }
        var longitude = reader.GetDecimal();

        reader.Read();
        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException("Expected latitude.");
        }
        var latitude = reader.GetDecimal();

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Unexpected data in coordinates array.");
        }

        return new GeolocationCoordinate { Longitude = longitude, Latitude = latitude };
    }

    public override void Write(Utf8JsonWriter writer, GeolocationCoordinate value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            throw new JsonException("Coordinates cannot be empty.");
        }

        var coordinate = value;
        writer.WriteStartArray();
        writer.WriteNumberValue(coordinate.Longitude);
        writer.WriteNumberValue(coordinate.Latitude);
        writer.WriteEndArray();
    }
}