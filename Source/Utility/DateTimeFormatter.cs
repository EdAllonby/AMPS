using System;

namespace Utility
{
    public static class DateTimeFormatter
    {
        private const int DaysInYear = 365;
        private static readonly int DaysInMonth = 30;

        public static string TimeAgo(this DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;

            if (span.Days > DaysInYear)
            {
                int years = (span.Days/DaysInYear);

                if (span.Days%DaysInYear != 0)
                {
                    years += 1;
                }

                return $"about {years} {(years == 1 ? "year" : "years")} ago";
            }
            if (span.Days > DaysInMonth)
            {
                int months = (span.Days/DaysInMonth);

                if (span.Days%31 != 0)
                {
                    months += 1;
                }

                return $"about {months} {(months == 1 ? "month" : "months")} ago";
            }
            if (span.Days > 0)
            {
                return $"about {span.Days} {(span.Days == 1 ? "day" : "days")} ago";
            }
            if (span.Hours > 0)
            {
                return $"about {span.Hours} {(span.Hours == 1 ? "hour" : "hours")} ago";
            }
            if (span.Minutes > 0)
            {
                return $"about {span.Minutes} {(span.Minutes == 1 ? "minute" : "minutes")} ago";
            }
            if (span.Seconds > 5)
            {
                return $"about {span.Seconds} seconds ago";
            }
            return "just now";
        }
    }
}