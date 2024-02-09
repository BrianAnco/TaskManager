namespace TaskManager.Responses
{
    public record ServiceResponse(bool Flag, string Message = null!);
    public record LoginResponse(bool Flag, string Message = null!);
}
