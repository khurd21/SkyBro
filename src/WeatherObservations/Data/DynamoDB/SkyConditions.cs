namespace WeatherObservations.Data.DynamoDB;

public class SkyConditions
{
    public string? SkyCover
    {
        get
        {
            string skyCover = _skyCover ?? string.Empty;
            if (SkyCoverMap.ContainsKey(skyCover))
            {
                return SkyCoverMap[skyCover];
            }
            return "Unknown";
        }

        init
        {
            _skyCover = value;
        }
    }

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

    private string? _skyCover { get; set; }

    private static Dictionary<string, string> SkyCoverMap { get; } = new()
    {
        { "CLR", "Clear" },
        { "FEW", "Few" },
        { "SCT", "Scattered" },
        { "BKN", "Broken" },
        { "OVC", "Overcast" },
        { "VV", "Vertical Visibility" }
    };
}