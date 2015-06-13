using System;

namespace Utility
{
    public static class TimeSpanFormatter
    {
        public static string ToDays(this TimeSpan timeSpan, bool capitalise)
        {
            const string Singular = "Day";
            const string Plural = "Days";
            const string SpecialCase = "today";

            return capitalise ? timeSpan.Days.AddSuffix(Singular, Plural, SpecialCase) : timeSpan.Days.AddSuffix(Singular.ToLower(), Plural.ToLower(), SpecialCase);
        }
    }
}