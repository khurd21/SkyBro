namespace SkyBro.Services;

public class APIResponse<T>
{
    public required T Data { get; init; }
    public required string Message { get; init; }
    public required bool Success { get; init; }
}