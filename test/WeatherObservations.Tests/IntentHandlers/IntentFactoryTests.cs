using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using Ninject.Parameters;
using WeatherObservations.IntentHandlers;
using WeatherObservations.IntentHandlers.Amazon;
using WeatherObservations.IntentHandlers.WeatherObservations;
using Xunit;

namespace WeatherObservations.Tests.IntentHandlers;

public class IntentFactoryTests
{
    IKernel MockKernel { get; set; }

    IntentFactory IntentFactory { get; set; }

    public IntentFactoryTests()
    {
        this.MockKernel = new MoqMockingKernel();
        this.IntentFactory = new(this.MockKernel);
    }

    [Theory]
    [InlineData("WeatherObservationsIntent", typeof(WeatherObservationsIntentHandler), typeof(IWeatherObservationsIntentHandler))]
    [InlineData("AMAZON", typeof(AmazonIntentHandler), typeof(IAmazonIntentHandler))]
    public void TestGetIntentHandler(string intentName, Type expectedType, Type interfaceType)
    {
        // Arrange
        this.MockKernel.Bind(interfaceType).To(expectedType);

        // Act
        var intentHandler = this.IntentFactory.GetIntentHandler(intentName);

        // Assert
        Assert.IsType(expectedType, intentHandler);
    }
}