using System;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day06
    {
        [Fact]
        public void Test1()
        {
            var input = @"
abc

a
b
c

ab
ac

a
a
a
a

b";
            Assert.Equal(11, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(6612, Part1(File.ReadAllText("input/day06.txt")));
        }

        [Fact]
        public void Test3()
        {
            var input = @"
abc

a
b
c

ab
ac

a
a
a
a

b";
            Assert.Equal(6, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(3268, Part2(File.ReadAllText("input/day06.txt")));
        }

        private static int Part1(string input) => ComputeSolution(input, (a, b) => a | b);

        private static int Part2(string input) => ComputeSolution(input, (a, b) => a & b);

        private static int ComputeSolution(string input, Func<int, int, int> aggregationFunc) =>
            string.Concat(input.Where(c => c != '\r'))
                .Split("\n\n")
                .Select(g => g.Split('\n').Where(s => s.Length > 0))
                .Select(g => g.Select(person =>
                {
                    int bitmap = 0;
                    foreach (var c in person)
                    {
                        bitmap |= (1 << (c - 'a'));
                    }
                    return bitmap;
                }))
                .Select(g => g.Aggregate(aggregationFunc))
                .Select(g => CountBits(g))
                .Sum();

        private static int CountBits(int input)
        {
            var ret = 0;
            while (input > 0)
            {
                ret += (input & 0x1);
                input >>= 1;
            }
            return ret;
        }
    }
}
