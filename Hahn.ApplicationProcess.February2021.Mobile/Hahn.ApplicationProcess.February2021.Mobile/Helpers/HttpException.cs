using System;

namespace Hahn.Mobile.Helpers
{
    public class HttpException : Exception
    {
        public HttpException(int code, string title, string message) : base(message)
        {
            Code = code;
            Title = title;
        }

        public HttpException(Exception exception) : base(exception.Message, exception)
        {
            Code = -1;
        }

        public int Code { get; set; }
        public string Title { get; set; }
    }
}
