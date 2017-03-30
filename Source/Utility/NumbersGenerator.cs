using System.Collections.Generic;

namespace Utility
{
    /// <summary>
    /// Helper for generating number patterns.
    /// </summary>
    public static class NumbersGenerator
    {
        /// <summary>
        /// Create a sequence of Fibonacci numbers.
        /// </summary>
        /// <param name="highestNumber">The highest number to output.</param>
        /// <param name="createUniqueList">Whether any duplicate numbers should be added.</param>
        /// <returns></returns>
        public static IEnumerable<int> CreateFibonacciSequence(int highestNumber, bool createUniqueList)
        {
            var fibonacciNumbers = new List<int>();

            int number;
            var lastFibonacciNumber = 0;
            var currentFibonacciNumber = 1;

            for (number = 0; number <= highestNumber; number++)
            {
                int nextFibonacciNumber = lastFibonacciNumber + currentFibonacciNumber;

                if (createUniqueList && !fibonacciNumbers.Contains(nextFibonacciNumber))
                {
                    fibonacciNumbers.Add(nextFibonacciNumber);
                }
                else
                {
                    fibonacciNumbers.Add(nextFibonacciNumber);
                }

                lastFibonacciNumber = currentFibonacciNumber;

                currentFibonacciNumber = nextFibonacciNumber;
            }

            return fibonacciNumbers;
        }

        /// <summary>
        /// Create a linear sequence of numbers based on its specification.
        /// </summary>
        /// <param name="index">The starting number.</param>
        /// <param name="count">How many numbers to add.</param>
        /// <param name="gap">The distance between the numbers.</param>
        /// <returns></returns>
        public static IEnumerable<int> CreateNumberSequence(int index, int count, int gap)
        {
            var numberSequence = new List<int>();

            int highestNumber = index + count * gap;

            for (int number = index; number < highestNumber; number = number + gap)
            {
                numberSequence.Add(number);
            }

            return numberSequence;
        }
    }
}