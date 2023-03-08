namespace WeatherObservations.Data;

public class WeatherData
{
    public string? StationID { get; init; }

    public string? RawText { get; init; }

    public string? FlightCategory { get; init; }

    public DateTime? ObservationTime { get; init; }

    public float? TemperatureCelsius { get; init; }

    public float? TemperatureFahrenheit
    {
        get
        {
            return TemperatureCelsius * 9 / 5 + 32;
        }
        init
        {
            TemperatureCelsius = (value - 32) * 5 / 9;
        }
    }

    public float? SeaLevelPressureMb { get; init; }

    public float? DewPointCelsius { get; init; }

    public float? DewPointFahrenheit
    {
        get
        {
            return DewPointCelsius * 9 / 5 + 32;
        }
        init
        {
            DewPointCelsius = (value - 32) * 5 / 9;
        }
    }

    public int? WindDirectionDegrees { get; init; }

    public int? WindSpeedMph
    {
        get
        {
            return (int?)(WindSpeedKnots * 1.15078);
        }
        init
        {
            WindSpeedKnots = (int?)(value / 1.15078);
        }
    }

    public int? WindGustMph
    {
        get
        {
            return (int?)(WindGustKnots * 1.15078);
        }
        init
        {
            WindGustKnots = (int?)(value / 1.15078);
        }
    }

    public int? WindSpeedKnots { get; init; }

    public int? WindGustKnots { get; init; }

    public float? VisibilityStatuteMiles { get; init; }

    public float? AltimeterInHg { get; init; }

    public float? PrecipitationInches { get; init; }

    public int? PrecipitationPercent { get; init; }

    public int? PrecipitationForSnowPercent { get; init; }

    public float? ElevationMeters { get; init; }

    public int? LightningPercent { get; init; }

    public IList<SkyConditions>? SkyConditions { get; init; }

    public bool IsGusting => WindGustKnots - WindSpeedKnots > GUST_THRESHOLD;

    public bool IsLightning => RawText?.Contains("LTG") ?? false ? true : LightningPercent > LIGHTNING_THRESHOLD;

    public WeatherData() { }

    public WeatherData(in string rawText) => RawText = rawText;

    public static int GUST_THRESHOLD { get; } = 5;

    public static int LIGHTNING_THRESHOLD { get; } = 10;

    public override string ToString()
    {
        string properties = $"<{nameof(WeatherData)}>\n";
        foreach (var prop in GetType().GetProperties())
        {
            properties += $"\t{prop.Name}: {prop.GetValue(this, null)}\n";
        }
        foreach (var item in SkyConditions ?? new List<SkyConditions>())
        {
            properties += $"\t{item}\n";
        }
        properties += $"<{nameof(WeatherData)} />";
        return properties;
    }
}