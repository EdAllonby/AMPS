using System;

namespace Utility
{
    public static class TimeSpanFormatter
    {
        public static string ToDays(this TimeSpan timeSpan, bool capitalise)
        {
            
            const string singular = "Day";
            const string plural = "Days";

            return capitalise ? ToNoun(timeSpan.Days, singular, plural) : ToNoun(timeSpan.Days, singular.ToLower(), plural.ToLower());
        }

        private static string ToNoun(int timeUnitDistance, string singularNoun, string pluralNoun)
        {
            NounState nounState;

            switch (timeUnitDistance)
            {
                case 0:
                    // Special case.
                    return "Today";
                case 1:
                    nounState = NounState.Singular;
                    break;
                default:
                    nounState = NounState.Plural;
                    break;
            }

            string nounToUse = NounFinder(nounState, singularNoun, pluralNoun);

            return string.Format("{0} {1}", timeUnitDistance, nounToUse);
        }

        private static string NounFinder(NounState nounState, string singularVersion, string pluralVersion)
        {
            switch (nounState)
            {
                case NounState.Singular:
                    return singularVersion;

                case NounState.Plural:
                    return pluralVersion;
            }

            // If no noun state specified, return the more common plural version (worst case will say 1 days).
            return pluralVersion;
        }

        private enum NounState
        {
            Singular,
            Plural
        }
    }
}