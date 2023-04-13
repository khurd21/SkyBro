using WeatherObservations.Data.DynamoDB;
using Xunit;

namespace WeatherObservations.Tests.Data.DynamoDB;

public class SkyConditionsTests
{
    [Fact]
    public void TestToString()
    {
        SkyConditions skyConditions = new()
        {
            SkyCover = "Clear",
            CloudBaseFeetAGL = 1000,
            CloudCoverPercent = 10,
        };

        string actual = skyConditions.ToString();

        Assert.Contains($"<{nameof(SkyConditions)}>\n", actual);
        Assert.Contains($"\t{nameof(SkyConditions.SkyCover)}: {skyConditions.SkyCover}\n", actual);
        Assert.Contains($"\t{nameof(SkyConditions.CloudBaseFeetAGL)}: {skyConditions.CloudBaseFeetAGL}\n", actual);
        Assert.Contains($"\t{nameof(SkyConditions.CloudCoverPercent)}: {skyConditions.CloudCoverPercent}\n", actual);
        Assert.Contains($"<{nameof(SkyConditions)} />", actual);
    }
}