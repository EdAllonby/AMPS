using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Shared
{
    /// <summary>
    /// Packages extensions for <see cref="Enumerable" />.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Checks if two sets contain the same items in any order.
        /// Example:
        /// If,	F = {20, 60, 80}
        /// And, G = {80, 60, 20}
        /// Then, F=G, that is both sets are equal.
        /// </summary>
        /// <typeparam name="TElement">Type of element in the collection.</typeparam>
        /// <param name="first">The first list of unordered items.</param>
        /// <param name="second">The second list of unordered items.</param>
        /// <returns>Boolean result of if lists contain same elements.</returns>
        public static bool AreSetsEqual<TElement>([NotNull] this IEnumerable<TElement> first, [NotNull] IEnumerable<TElement> second)
        {
            IReadOnlyDictionary<TElement, int> firstMap = FindElementGroups(first);
            IReadOnlyDictionary<TElement, int> secondMap = FindElementGroups(second);

            return MapContainsAllElements(firstMap, secondMap)
                   && MapContainsAllElements(secondMap, firstMap);
        }

        private static IReadOnlyDictionary<TElement, int> FindElementGroups<TElement>(IEnumerable<TElement> elements)
        {
            return elements.GroupBy(element => element).ToDictionary(elementGroup => elementGroup.Key, grouping => grouping.Count());
        }

        private static bool MapContainsAllElements<TElement>(IReadOnlyDictionary<TElement, int> firstMap, IReadOnlyDictionary<TElement, int> secondMap)
        {
            return firstMap.Keys.All(IsElementInBothMaps(firstMap, secondMap));
        }

        private static Func<TElement, bool> IsElementInBothMaps<TElement>(IReadOnlyDictionary<TElement, int> firstMap, IReadOnlyDictionary<TElement, int> secondMap)
        {
            return element => secondMap.Keys.Contains(element) && firstMap[element].Equals(secondMap[element]);
        }
    }
}