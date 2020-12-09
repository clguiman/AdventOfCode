using System;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day09
    {
        [Fact]
        public void Test1()
        {
            var input = new long[] { 35, 20, 15, 25, 47, 40, 62, 55, 65, 95, 102, 117, 150, 182, 127, 219, 299, 277, 309, 576 };
            Assert.Equal(127, Part1(input, 5));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(15690279, Part1(File.ReadAllLines("input/day09.txt").Select(long.Parse).ToArray()));
        }

        [Fact]
        public void Test3()
        {
            var input = new long[] { 35, 20, 15, 25, 47, 40, 62, 55, 65, 95, 102, 117, 150, 182, 127, 219, 299, 277, 309, 576 };
            Assert.Equal(62, Part2(input, 127));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(2174232, Part2(File.ReadAllLines("input/day09.txt").Select(long.Parse).ToArray(), 15690279));
        }

        private static long Part1(long[] numbers, int preamble = 25)
        {
            for (var i = preamble; i < numbers.Length; i++)
            {
                if (!IsNumberAPreviousSum(numbers[i], numbers.AsSpan(i - preamble, preamble)))
                {
                    return numbers[i];
                }
            }
            return 0;
        }

        private static bool IsNumberAPreviousSum(long n, Span<long> range)
        {
            for (var i = 0; i < range.Length; i++)
            {
                for (var j = i + 1; j < range.Length; j++)
                {
                    if ((range[i] + range[j]) == n)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static long Part2(long[] numbers, long sum)
        {
            for (var i = 0; i < numbers.Length - 2; i++)
            {
                long tempSum = numbers[i] + numbers[i + 1];
                var min = numbers[i] > numbers[i + 1] ? numbers[i + 1] : numbers[i];
                var max = numbers[i] > numbers[i + 1] ? numbers[i] : numbers[i + 1];
                for (var j = i + 2; tempSum < sum && j < numbers.Length; j++)
                {
                    tempSum += numbers[j];
                    min = min > numbers[j] ? numbers[j] : min;
                    max = max < numbers[j] ? numbers[j] : max;
                }
                if (tempSum == sum)
                {
                    return min + max;
                }
            }
            return -1;
        }
    }
}
