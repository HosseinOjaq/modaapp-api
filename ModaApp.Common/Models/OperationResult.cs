using ModaApp.Common.Enums;
using ModaApp.Common.Extensions;

namespace ModaApp.Common.Models;

public class OperationResult(bool isSuccess, OperationStatusCode status, params ErrorModel[] messages)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public OperationStatusCode Status { get; set; } = status;
    public ErrorModel[] Messages { get; set; } = messages;


    public static implicit operator OperationResult(ErrorModel message)
      => new(false, OperationStatusCode.OK, message);

    public static OperationResult Fail(string message)
        => new(false, OperationStatusCode.ServerError, ErrorModel.Create("ServerError", message));

    public static OperationResult Fail()
        => new(false, OperationStatusCode.ServerError, ErrorModel.Create("ServerError", OperationStatusCode.ServerError.ToDisplay()));

    public static OperationResult Success()
        => new(true, OperationStatusCode.OK, ErrorModel.Create("OK", OperationStatusCode.OK.ToDisplay()));
}

public class OperationResult<TData>
    (bool isSuccess, OperationStatusCode status, TData? data, params ErrorModel[] messages)
    : OperationResult(isSuccess, status, messages)
{
    public TData? Result { get; set; } = data;


    public static implicit operator OperationResult<TData>(TData value)
        => new(true, OperationStatusCode.OK, value);

    public static implicit operator OperationResult<TData>(ErrorModel error)
      => new(false, OperationStatusCode.OK, default, error);

    public static implicit operator OperationResult<TData>(ErrorModel[] errors)
      => new(false, OperationStatusCode.OK, default, errors);

    public static OperationResult<TData> Fail(TData? data = default)
        => new(false, OperationStatusCode.ServerError, data, ErrorModel.Create("ServerError", OperationStatusCode.ServerError.ToDisplay()));
}