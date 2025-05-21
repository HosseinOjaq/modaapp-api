namespace ModaApp.Common.Models;

public record ErrorModel
{
    public string Code { get; private set; } = default!;
    public string Message { get; private set; } = default!;


    public static ErrorModel Create(string code, string message)
        => new()
        {
            Code = code,
            Message = message
        };
};