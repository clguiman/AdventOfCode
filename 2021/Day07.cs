using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day07
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(37, Part1(new[] { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14 }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(352331, Part1(File.ReadAllText("input/day07.txt").Split(',').Select(int.Parse)));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(168, Part2(new[] { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14 }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(99266250, Part2(File.ReadAllText("input/day07.txt").Split(',').Select(int.Parse)));
        }

        private static int Part1(IEnumerable<int> input)
        {
            var median = input.OrderBy(x => x).Skip((input.Count() / 2) ).First();
            return input.Select(x => Math.Abs(x - median)).Sum();
        }
        private static int Part2(IEnumerable<int> input)
        {
            return Enumerable.Range((int)input.Average() - 1, 4).Select(newPos => input.Select(x =>
            {
                var d = Math.Abs(x - newPos);
                return (d * (d + 1)) / 2;
            }).Sum()).Min();
        }
    }
}
