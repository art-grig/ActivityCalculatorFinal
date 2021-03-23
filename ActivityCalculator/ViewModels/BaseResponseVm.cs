using ActivityCalculator.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityCalculator.ViewModels
{
    public class BaseResponseVm
    {
        public bool Success { get; set; }
        public DateTime GeneratedAt { get; }
        public string? UserMessage { get; set; }
        public string? SystemMessage { get; set; }

        public BaseResponseVm()
        {
            GeneratedAt = DateTime.UtcNow;
            Success = true;
        }

        public BaseResponseVm(Exception exception, string? userMessage = null)
            : this()
        {
            Success = false;
            if (exception is AppException ex)
            {
                UserMessage = userMessage ?? ex.Message;
                SystemMessage = ex.SystemMessage;
            }
            else
            {
                UserMessage = "Error occured while processing request, please contact technical support";
                SystemMessage = exception.Message;
            }
        }
    }
}
