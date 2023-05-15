using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace WeatherObservations.IntegrationTests.Steps;

[Binding]
public sealed class RequestTypeStepDefinitions
{
    private StepStateHandler StateHandler { get; init; }

    private ITestOutputHelper OutputHelper { get; init; }

    private static string ResponseSnippetColumn { get; } = "ResponseSnippet";

    public RequestTypeStepDefinitions(StepStateHandler stateHandler, ITestOutputHelper outputHelper)
    {
        this.StateHandler = stateHandler;
        this.OutputHelper = outputHelper;
    }

    [Given(@"the user asks for ""([^\""]*)""")]
    public void GivenUserAsksFor(string request)
    {
        this.StateHandler.SetRequestType(request);
    }

    [When(@"the request is made")]
    public void GivenTheRequestIsComplete()
    {
        this.StateHandler.SendRequest();
    }

    [Then(@"the response should contain")]
    public void ThenTheResponseShouldContain(Table table)
    {
        // TODO: This is specific to HELP intent. Needs to be generic
        IEnumerable<string> responseSnippets = AssertRequestAndProcessTable(table, ResponseSnippetColumn);
        this.AssertContains(responseSnippets, "response.outputSpeech.text");
    }

    [Then(@"the reprompt should contain")]
    public void ThenTheRepromptShouldContain(Table table)
    {
        IEnumerable<string> responseSnippets = AssertRequestAndProcessTable(table, ResponseSnippetColumn);
        this.AssertContains(responseSnippets, "response.reprompt.outputSpeech.text");
    }

    private void AssertContains(IEnumerable<string> subsets, string token)
    {
        string actual = this.StateHandler.OutputPayload
            .SelectToken(token)
            .ToString();

        this.OutputHelper.WriteLine(this.StateHandler.OutputPayload.ToString());

        Assert.All(subsets, delegate(string subset)
        {
            this.OutputHelper.WriteLine(
                $"Expected substring: {subset}\n" +
                $"Actual string: {actual}\n" + 
                "------\n"
            );
            Assert.Contains(subset, actual);
        });
    }

    private IEnumerable<string> AssertRequestAndProcessTable(Table table, string columnName)
    {
        Assert.True(this.StateHandler.IsRequestComplete, "TEST ERROR: The request has not completed.");
        Assert.True(table.ContainsColumn(columnName), $"TEST ERROR: Table does not contain: {columnName}");
        return table.Rows.Select(row => row[columnName]);
    }
}