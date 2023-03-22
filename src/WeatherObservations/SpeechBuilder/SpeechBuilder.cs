namespace WeatherObservations.SpeechBuilder;

public class SpeechBuilder
{
    private string Speech { get; set; }

    public SpeechBuilder()
    {
        this.Speech = string.Empty;
    }

    protected void Add(string text)
    {
        this.Speech += text.Trim() + " ";
    }

    public string Build() => this.Speech.Trim();
}