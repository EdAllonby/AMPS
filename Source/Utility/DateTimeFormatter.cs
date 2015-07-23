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

                return string.Format("about {0} {1} ago", years, years == 1 ? "year" : "years");
            }
            if (span.Days > DaysInMonth)
            {
                int months = (span.Days/DaysInMonth);

                if (span.Days%31 != 0)
                {
                    months += 1;
                }

                return string.Format("about {0} {1} ago", months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
            {
                return string.Format("about {0} {1} ago", span.Days, span.Days == 1 ? "day" : "days");
            }
            if (span.Hours > 0)
            {
                return string.Format("about {0} {1} ago", span.Hours, span.Hours == 1 ? "hour" : "hours");
            }
            if (span.Minutes > 0)
            {
                return string.Format("about {0} {1} ago", span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            }
            if (span.Seconds > 5)
            {
                return string.Format("about {0} seconds ago", span.Seconds);
            }
            return "just now";
        }
    }
}