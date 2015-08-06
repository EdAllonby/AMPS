namespace Utility
{
    public static class NumberFormatter
    {
        public static string AddSuffix(this int number, string singularSuffix, string pluralSuffix, string specialCase = "", bool capitalise = false)
        {
            return capitalise ? ToNoun(number, singularSuffix, pluralSuffix, specialCase) : ToNoun(number, singularSuffix.ToLower(), pluralSuffix.ToLower(), specialCase);
        }

        private static string ToNoun(int timeUnitDistance, string singularNoun, string pluralNoun, string specialCase)
        {
            NounState nounState;

            switch (timeUnitDistance)
            {
                case 0:
                    // Special case.
                    if (string.IsNullOrWhiteSpace(specialCase))
                    {
                        nounState = NounState.Plural;
                        break;
                    }

                    return specialCase;
                case 1:
                    nounState = NounState.Singular;
                    break;
                default:
                    nounState = NounState.Plural;
                    break;
            }

            string nounToUse = NounFinder(nounState, singularNoun, pluralNoun);

            return $"{timeUnitDistance} {nounToUse}";
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