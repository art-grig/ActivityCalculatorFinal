using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityCalculator.Services.Extensions
{
    public static class DateTimeExtensions
    {
        public static int DaysDiff(this DateTime from, DateTime to)
        {
            return Math.Abs(Convert.ToInt32((to - from).TotalDays));
        }
    }
}
