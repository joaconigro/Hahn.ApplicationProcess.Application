using System;

namespace Hahn.Mobile.Dtos
{
    public class ErrorDetailsDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ErrorDetailsDto(int code, Exception exception)
        {
            StatusCode = code;
            Message = exception.Message + Environment.NewLine + exception.StackTrace;
        }

        public ErrorDetailsDto(int code, string message)
        {
            StatusCode = code;
            Message = message;
        }
    }
}
