namespace WeatherObservations;

public class WeatherData
{
    public string? StationID { get; init; }

    public string? RawText { get; init; }

    public string? FlightCategory { get; init; }

    public DateTime? ObservationTime { get; init; }

    public float? TemperatureCelsius { get; init; }

    public float? SeaLevelPressureMb { get; init; }

    public float? DewPointCelsius { get; init; }

    public int? WindDirectionDegrees { get; init; }

    public int? WindSpeedKnots { get; init; }

    public float? VisibilityStatuteMiles { get; init; }

    public float? AltimeterInHg { get; init; }

    public int? WindGustKnots { get; init; }

    public float? PrecipitationInches { get; init; }

    public float? ElevationMeters { get; init; }

    public IList<SkyConditions>? SkyConditions { get; init; }

    public bool IsGusting => this.WindGustKnots - this.WindSpeedKnots > GUST_THRESHOLD;

    public bool IsLightning => RawText?.Contains("LTG") ?? false;

    public WeatherData() { }

    public WeatherData(in string rawText) => this.RawText = rawText;

    public static int GUST_THRESHOLD { get; } = 10;

    public override string ToString()
    {
        string properties = $"<{nameof(WeatherData)}>\n";
        foreach (var prop in this.GetType().GetProperties())
        {
            properties += $"\t{prop.Name}: {prop.GetValue(this, null)}\n";
        }
        foreach (var item in this.SkyConditions ?? new List<SkyConditions>())
        {
            properties += $"\t{item}\n";
        }
        properties += $"<{nameof(WeatherData)} />";
        return properties;
    }
}