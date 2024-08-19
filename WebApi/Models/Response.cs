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
        public string? Error { get; set; }

        public Response()
        {
        }

        public Response(bool success, string message, T? data = default, string? error = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Error = error;
        }

        public static Response<T> SuccessResponse(T? data, string message = "İşlem başarılı.")
        {
            return new Response<T>(true, message, data);
        }

        public static Response<T> ErrorResponse(string error, string message = "Bir hata oluştu")
        {
            return new Response<T>(false, message, error: error);
        }
    }
}