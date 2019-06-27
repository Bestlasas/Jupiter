using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter.Helpers.Models.Utils
{
    public class Error
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }

    public class Result
    {
        public bool Success { get; set; } = false;
       
        public List<Error> Errors { get; set; }

        public Result()
        {
            Success = false;
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}
