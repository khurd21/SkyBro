using Xunit;

namespace SkyBro.Services.Tests;
public class APIResponseTests
{
    [Fact]
    public void APIResponse_InitializesPropertiesCorrectly()
    {
        // Arrange
        var expectedData = new { Name = "Test" };
        const string expectedMessage = "Operation completed successfully.";
        const bool expectedSuccess = true;

        // Act
        var response = new APIResponse<object>
        {
            Data = expectedData,
            Message = expectedMessage,
            Success = expectedSuccess
        };

        // Assert
        Assert.Equal(expectedData, response.Data);
        Assert.Equal(expectedMessage, response.Message);
        Assert.Equal(expectedSuccess, response.Success);
    }

    [Theory]
    [InlineData(true, "Success")]
    [InlineData(false, "Failure")]
    public void APIResponse_HandlesDifferentSuccessStates(bool success, string message)
    {
        // Arrange
        var data = new { Value = 42 };

        // Act
        var response = new APIResponse<object>
        {
            Data = data,
            Message = message,
            Success = success
        };

        // Assert
        Assert.Equal(data, response.Data);
        Assert.Equal(message, response.Message);
        Assert.Equal(success, response.Success);
    }
}