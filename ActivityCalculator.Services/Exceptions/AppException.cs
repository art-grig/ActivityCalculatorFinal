using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityCalculator.Services.Exceptions
{
    public class AppException : Exception
    {
        public string? SystemMessage { get; set; }

        public AppException(string userMessage, string? systemMessage = null) : base(userMessage)
        {
            SystemMessage = systemMessage;
        }
    }
}
