using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day05
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal((44, 5), ParseSeat("FBFBBFFRLR"));
            Assert.Equal((70, 7), ParseSeat("BFFFBBFRRR"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(818, Part1(File.ReadLines("input/day05.txt")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(559, Part2(File.ReadLines("input/day05.txt")));
        }

        static int Part1(IEnumerable<string> tickets) => ParseSeatIds(tickets).Max();

        static int Part2(IEnumerable<string> tickets)
        {
            var ids = ParseSeatIds(tickets);
            var min = ids.Min();
            var max = ids.Max();

            return (max * (max + 1) - min * (min - 1)) / 2 - ids.Sum();
        }

        static IEnumerable<int> ParseSeatIds(IEnumerable<string> tickets) => tickets
                .Select(t => ParseSeat(t))
                .Select(i => i.row * 8 + i.column);

        static (int row, int column) ParseSeat(string input)
        {
            int row = 0;
            int column = 0;
            int pow = 1;
            for (var i = 6; i >= 0; i--, pow *= 2)
            {
                row += pow * (input[i] == 'B' ? 1 : 0);
            }
            pow = 1;
            for (var i = 9; i > 6; i--, pow *= 2)
            {
                column += pow * (input[i] == 'R' ? 1 : 0);
            }
            return (row, column);
        }
    }
}
