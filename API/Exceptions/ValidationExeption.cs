using System;
using System.Collections.Generic;

namespace API.Exceptions
{
    public class ValidationExeption : Exception
    {
        public ValidationExeption(string message, List<string> errors) : base(message)
        {
        }

        public ValidationExeption(string message) : base(message)
        {
        }
    }
}