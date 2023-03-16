namespace WeatherObservations.Data.DynamoDB;

public class SkyConditions
{
    public string? SkyCover { get; init; }

    public int? CloudBaseFeetAGL { get; init; }

    public int? CloudCoverPercent { get; init; }

    public override string ToString()
    {
        string result = $"<{nameof(SkyConditions)}>\n";
        foreach (var prop in GetType().GetProperties())
        {
            result += $"\t{prop.Name}: {prop.GetValue(this, null)}\n";
        }
        result += $"<{nameof(SkyConditions)} />";
        return result;
    }
}