using System;

namespace BlogSM.API.Services.Models;

public class ServiceResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public ServiceResponse(bool success)
    {
        Success = success;
    }

    public ServiceResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public ServiceResponse(bool success, string message, T data)
    {
        Success = success;
        Message = message;
        Data = data;
    }
}
