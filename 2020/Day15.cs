using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day15
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(436, Part1(new long[] { 0, 3, 6 }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(1325, Part1(new long[] { 19L, 20L, 14, 0, 9, 1 }));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(175594, Part2(new long[] { 0, 3, 6 }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(59006, Part2(new long[] { 19, 20, 14, 0, 9, 1 }));
        }

        private static long Part1(IEnumerable<long> input) => Solution(input, 2020);

        private static long Part2(IEnumerable<long> input) => Solution(input, 30_000_000);

        private static long Solution(IEnumerable<long> input, int limit)
        {
            var spokenNumers = input
                .Select((x, idx) => (number: x, idx: (long)idx, prevIdx: -1L))
                .ToDictionary(item => item.number);

            var lastEntry = spokenNumers[input.Last()];
            for (long i = input.Count(); i < limit; i++)
            {
                var nextSpokenNumber = lastEntry.prevIdx == -1L ? 0L : i - 1 - lastEntry.prevIdx;

                if (spokenNumers.TryGetValue(nextSpokenNumber, out var nextEntry))
                {
                    nextEntry.prevIdx = nextEntry.idx;
                    nextEntry.idx = i;
                    spokenNumers[nextSpokenNumber] = nextEntry;
                } 
                else
                {
                    nextEntry = (nextSpokenNumber, i, -1L);
                    spokenNumers.Add(nextSpokenNumber, nextEntry);
                }

                lastEntry = nextEntry;
            }
            return lastEntry.number;
        }
    }
}
