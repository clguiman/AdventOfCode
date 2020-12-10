using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day10
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4 };
            Assert.Equal(35, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(1625, Part1(File.ReadAllLines("input/day10.txt").Select(int.Parse)));
        }

        [Fact]
        public void Test3()
        {
            var input = new[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4 };
            Assert.Equal(8, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(3100448333024, Part2(File.ReadAllLines("input/day10.txt").Select(int.Parse)));
        }

        private static int Part1(IEnumerable<int> input)
        {
            int oneDiff = 0;
            int threeDiff = 0;
            int prev = 0;
            foreach (var n in input
                .OrderBy(x => x)
                .Append(input.Max() + 3))
            {
                if (n - prev == 1)
                {
                    oneDiff++;
                }
                else if (n - prev == 3)
                {
                    threeDiff++;
                }
                prev = n;
            }
            return oneDiff * threeDiff;
        }

        private static long Part2(IEnumerable<int> input)
        {
            return ComputeCombinations(
                input
                .OrderBy(x => x)
                .Prepend(0)
                .Append(input.Max() + 3)
                .ToArray()
                .AsSpan(), new());
        }

        private static long ComputeCombinations(Span<int> numbers, Dictionary<int, long> resultsCache)
        {
            if (numbers.Length == 1)
            {
                return 1;
            }
            if (resultsCache.ContainsKey(numbers.Length))
            {
                return resultsCache[numbers.Length];
            }

            var sum = 0L;
            for(var i = 1; i < numbers.Length && (numbers[i] - numbers[0]) <= 3; i++)
            {
                sum += ComputeCombinations(numbers[i..], resultsCache);
            }

            resultsCache.Add(numbers.Length, sum);
            return sum;
        }
    }
}
