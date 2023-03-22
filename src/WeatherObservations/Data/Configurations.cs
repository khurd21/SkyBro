namespace WeatherObservations.Data;

public sealed class Configurations
{
    // DynamoDB Configurations //
    public static string DYNAMODB_LOCAL_SERVICE_URL { get; } = "http://localhost:8000";

    public static bool DYNAMODB_CONSISTENT_READ { get; } = true;

    public static bool DYNAMODB_IGNORE_NULL_VALUES { get; } = true;

    public static bool DYNAMODB_SKIP_VERSION_CHECK { get; } = true;

    public static string? DYNAMODB_TABLE_NAME_PREFIX { get; } = null;

    public static int DYNAMODB_HOURS_TO_KEEP_OBSERVATIONS { get; } = 3;

    // Weather Data Configurations //

    public static int WEATHER_OBSERVATIONS_MINIMUM_TIME_FRAME { get; } = 7;

    public static int WEATHER_OBSERVATIONS_MAXIMUM_TIME_FRAME { get; } = 20;

    public static int WEATHER_OBSERVATIONS_LIGHT_AND_VARIABLE_WIND_THRESHOLD { get; } = 7;

    // System Configurations //
    public static bool IS_DEBUG { get; } = IsDebug();

    private static bool IsDebug()
    {
        #if DEBUG
            return true;
        #else
            return false;
        #endif
    }
}