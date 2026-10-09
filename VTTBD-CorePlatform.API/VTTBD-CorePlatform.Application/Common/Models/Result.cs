using System.Collections.Generic;

namespace VTTBD_CorePlatform.Application.Common.Models;

public class Result
{
    public bool IsSuccess { get; }
    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }

    protected Result(bool isSuccess, string? errorCode = null, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public static Result Success() => new(true);
    public static Result Failure(string errorCode, string? errorMessage = null)
        => new(false, errorCode, errorMessage);
}

public class Result<T> : Result
{
    public T? Data { get; }

    private Result(bool isSuccess, T? data = default, string? errorCode = null, string? errorMessage = null)
        : base(isSuccess, errorCode, errorMessage)
    {
        Data = data;
    }

    public static Result<T> Success(T data) => new(true, data);
    public new static Result<T> Failure(string errorCode, string? errorMessage = null)
        => new(false, default, errorCode, errorMessage);
}