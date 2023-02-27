using System.Net;

namespace WeatherObservations;

public static class WeatherObservationsGlobals
{

    public static string URL_FOR_WEATHER { get; } = "https://www.aviationweather.gov/adds/dataserver_current/" +
        "httpparam?dataSource=metars&requestType=retrieve&format=xml&hoursBeforeNow" +
        "=5&mostRecentForEachStation=true&stationString=";

    public static string URL_FOR_US_AIRNET(string stationID, string state) => $"https://www.usairnet.com/cgi-bin/launch/code.cgi?sta={stationID}&state={state}";

    public static string STATION_ID { get; } = "station_id";

    public static string RAW_TEXT { get; } = "raw_text";

    public static string OBSERVATION_TIME { get; } = "observation_time";

    public static string LATITUDE { get; } = "latitude";

    public static string TEMPERATURE_CELSIUS { get; } = "temp_c";

    public static string DEWPOINT_CELSIUS { get; } = "dewpoint_c";

    public static string WIND_DIRECTION_DEGREES { get; } = "wind_dir_degrees";

    public static string WIND_SPEED_KNOTS { get; } = "wind_speed_kt";

    public static string WIND_GUST_KNOTS { get; } = "wind_gust_kt";

    public static string VISIBILITY_STATUTE_MILES { get; } = "visibility_statute_mi";

    public static string ALTIMETER_IN_HG { get; } = "altim_in_hg";

    public static string SEA_LEVEL_PRESSURE_MB { get; } = "sea_level_pressure_mb";

    public static string SKY_CONDITIONS { get; } = "sky_condition";

    public static string SKY_COVER { get; } = "sky_cover";

    public static string CLOUD_BASE_FEET_AGL { get; } = "cloud_base_ft_agl";

    public static string FLIGHT_CATEGORY { get; } = "flight_category";

    public static string ELEVATION_METERS { get; } = "elevation_m";

    public static string PRECIPITATION_INCHES { get; } = "precip_in";
}