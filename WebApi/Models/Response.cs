using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public Response()
        {
        }

        public Response(bool success, string message, T? data = default, List<string>? errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
        }

        public static Response<T> SuccessResponse(T ?data, string message = "Operation successful.")
        {
            return new Response<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static Response<T> ErrorResponse(List<string> errors, string message = "An error occurred.")
        {
            return new Response<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }

        public static Response<T> ErrorResponse(string error, string message = "An error occurred.")
        {
            return new Response<T>
            {
                Success = false,
                Message = message,
                Errors = [error]
            };
        }
    }
}