using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2022
{
    public class Day04
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(2, Part1(ParseInput(new[] {
                "2-4,6-8",
                "2-3,4-5",
                "5-7,7-9",
                "2-8,3-7",
                "6-6,4-6",
                "2-6,4-8",
            })));
        }
        [Fact]
        public void Test2()
        {
            Assert.Equal(444, Part1(ParseInput(File.ReadAllLines("input/day04.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(4, Part2(ParseInput(new[] {
                "2-4,6-8",
                "2-3,4-5",
                "5-7,7-9",
                "2-8,3-7",
                "6-6,4-6",
                "2-6,4-8",
            })));
        }
        [Fact]
        public void Test4()
        {
            Assert.Equal(801, Part2(ParseInput(File.ReadAllLines("input/day04.txt"))));
        }

        private static int Part1(IEnumerable<(Range first, Range second)> input)
        {
            return input.Count(x =>
            (x.first.Low <= x.second.Low && x.first.High >= x.second.High) ||
            (x.first.Low >= x.second.Low && x.first.High <= x.second.High));
        }

        private static int Part2(IEnumerable<(Range first, Range second)> input)
        {
            return input.Count(x =>
            (x.first.Low >= x.second.Low && x.first.Low <= x.second.High) ||
            (x.first.High >= x.second.Low && x.first.High <= x.second.High) ||
            (x.second.Low >= x.first.Low && x.second.Low <= x.first.High) ||
            (x.second.High >= x.first.Low && x.second.High <= x.first.High));
        }

        private static IEnumerable<(Range first, Range second)> ParseInput(IEnumerable<string> input)
        {
            var numbers = input.SelectMany(line => line.Split(',')).SelectMany(x => x.Split('-')).Select(int.Parse).ToArray();
            for (var idx = 0; idx < numbers.Length; idx += 4)
            {
                var first = new Range() { Low = numbers[idx], High = numbers[idx + 1] };
                var second = new Range() { Low = numbers[idx + 2], High = numbers[idx + 3] };
                if (first.Low <= second.Low)
                {
                    yield return (first, second);
                }
                else
                {
                    yield return (second, first);
                }
            }
        }

        private record Range
        {
            public int Low { get; init; }
            public int High { get; init; }
        }
    }
}