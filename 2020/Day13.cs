using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day13
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { "939", "7,13,x,x,59,x,31,19" };
            Assert.Equal(295, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(3464, Part1(File.ReadAllLines("input/day13.txt")));
        }

        [Fact]
        public void Test3()
        {
            var input = new[] { "939", "2,9" };
            Assert.Equal(8, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            var input = new[] { "939", "7,13,x,x,59,x,31,19" };
            Assert.Equal(1068781, Part2(input));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(760171380521445, Part2(File.ReadAllLines("input/day13.txt")));
        }

        private static long Part1(IEnumerable<string> input)
        {
            var timestamp = long.Parse(input.First());
            var busses = input.Last().Split(',').Where(x => x != "x").Select(long.Parse).ToArray();
            var closest = busses[0];
            var minTime = busses[0] - timestamp % busses[0];
            foreach (var bus in busses)
            {
                var waitTime = bus - timestamp % bus;
                if (waitTime < minTime)
                {
                    minTime = waitTime;
                    closest = bus;
                }
            }
            return minTime * closest;
        }

        private static long Part2(IEnumerable<string> input) => ChineseRemainderTheorem.Solve(
            input.Last().Split(',')
                .Select((x, idx) => x == "x" ? (-1L, -1L) : ((long)idx, long.Parse(x)))
                .Where(x => x.Item1 != -1L)
                .Select(item => (n: item.Item2, a: item.Item2 - item.Item1)));

        // https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
        public static class ChineseRemainderTheorem
        {
            public static long Solve(IEnumerable<(long n, long a)> input)
            {
                long prod = input.Aggregate(1L, (x, y) => x * y.n);
                long p;
                long sm = 0;
                foreach (var (n, a) in input)
                {
                    p = prod / n;
                    sm += a * ModularMultiplicativeInverse(p, n) * p;
                }
                return sm % prod;
            }

            private static long ModularMultiplicativeInverse(long a, long mod)
            {
                long b = a % mod;
                for (long x = 1; x < mod; x++)
                {
                    if (b * x % mod == 1)
                    {
                        return x;
                    }
                }
                return 1L;
            }
        }
    }
}
