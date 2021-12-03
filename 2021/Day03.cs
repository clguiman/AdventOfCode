using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day03
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(198, Part1(new[] {
                "00100",
                "11110",
                "10110",
                "10111",
                "10101",
                "01111",
                "00111",
                "11100",
                "10000",
                "11001",
                "00010",
                "01010"
            }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(3912944, Part1(File.ReadAllLines("input/day03.txt")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(230, Part2(new[] {
                "00100",
                "11110",
                "10110",
                "10111",
                "10101",
                "01111",
                "00111",
                "11100",
                "10000",
                "11001",
                "00010",
                "01010"
            }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(4996233, Part2(File.ReadAllLines("input/day03.txt")));
        }

        private static int Part1(string[] input)
        {
            var n = input[0].Length;
            int gamma = 0;
            for (var idx = 0; idx < n; idx++)
            {
                var count = input.Sum(x => x[idx] - '0');
                gamma <<= 1;
                if (count > input.Length / 2)
                {
                    gamma += 1;
                }
            }
            return gamma * (~gamma & ((1 << n) - 1));
        }

        private static int Part2(string[] input)
        {
            var n = input[0].Length;
            var mostCommon = input;
            var leastCommon = input;
            for (var idx = 0; idx < n; idx++)
            {
                if (mostCommon.Length > 1)
                {
                    mostCommon = MostLeastCommonOnBitPossition(mostCommon, idx, true).ToArray();
                }
                if (leastCommon.Length > 1)
                {
                    leastCommon = MostLeastCommonOnBitPossition(leastCommon, idx, false).ToArray();
                }
            }
            return Convert.ToInt32(mostCommon.First(), 2) * Convert.ToInt32(leastCommon.First(), 2);
        }

        private static IEnumerable<string> MostLeastCommonOnBitPossition(string[] input, int position, bool findMostCommon)
        {
            var ones = input.Sum(x => x[position] - '0');
            var zeros = input.Length - ones;
            var bitToKeep = (ones >= zeros) ? (findMostCommon ? '1' : '0') : (findMostCommon ? '0' : '1');
            return input.Where(x => x[position] == bitToKeep);
        }

    }
}
