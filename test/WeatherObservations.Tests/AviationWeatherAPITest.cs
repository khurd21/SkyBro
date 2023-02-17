using Xunit;

namespace WeatherObservations.Tests;

public class AviationWeatherAPITest
{
    [Theory]
    [InlineData("KSHN", "KSHN")]
    [InlineData("KCLS", "KCLS")]
    [InlineData("KHQM", "KHQM")]
    [InlineData("KSHN,KHQM", "KSHN")]
    public void TestGetSkyConditionsStringValidStationID(string stationId, string expectedStationId)
    {
        var skyConditions = Task.Run(async () => await AviationWeatherAPI.GetSkyConditions(stationId)).Result;
        Assert.NotNull(skyConditions);
        Assert.Equal(expectedStationId, skyConditions.StationID);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("GARBAGE DATA")]
    public void TestGetSkyConditionsStringInvalidStationID(string stationId)
    {
        var skyConditions = Task.Run(async () => await AviationWeatherAPI.GetSkyConditions(stationId)).Result;
        Assert.Null(skyConditions);
    }

    [Theory]
    [MemberData(nameof(GetSkyConditionsListOfStringsData))]
    public void TestGetSkyConditionsListOfStrings(IList<string> stationIds, IList<string> expectedStationIds)
    {

    }

    public static IEnumerable<object[]> GetSkyConditionsListOfStringsData()
    {
        yield return new object[] { new List<string>() { "KSHN" }, new List<string>() { "KSHN" } };
        yield return new object[] { new List<string>() { "KSHN", "KHQM" }, new List<string>() { "KSHN", "KHQM" } };
        yield return new object[] { new List<string>() { "KSHN", "KHQM", "KSHN" }, new List<string>() { "KSHN", "KHQM" } };
    }
}