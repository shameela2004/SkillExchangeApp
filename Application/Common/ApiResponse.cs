using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }             // Include status code
        public string? Message { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }                // Error message

        public ApiResponse(bool success, int statusCode, T? data = default, string? message = null, string? error = null)
        {
            Success = success;
            StatusCode = statusCode;
            Data = data;
            Message = message;
            Error = error;
        }

        public static ApiResponse<T> SuccessResponse(T data, int statusCode, string? message = null)
        {
            return new ApiResponse<T>(true, statusCode, data, message);
        }

        public static ApiResponse<T> FailResponse(int statusCode, string error, string? message = null)
        {
            return new ApiResponse<T>(false, statusCode, default, message, error);
        }
    }

}
