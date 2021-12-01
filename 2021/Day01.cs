using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day01
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 };
            Assert.Equal(7, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(1462, Part1(File.ReadAllLines("input/day01.txt").Select(int.Parse)));
        }

        [Fact]
        public void Test3()
        {
            var input = new[] { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 };
            Assert.Equal(5, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(1497, Part2(File.ReadAllLines("input/day01.txt").Select(int.Parse)));
        }

        private static int Part1(IEnumerable<int> input) => input.Skip(1).Zip(input).Count(x => x.First > x.Second);

        private static int Part2(IEnumerable<int> input) => Part1(input.Skip(1).Zip(input).Skip(1).Zip(input).Select(x => x.First.First + x.First.Second + x.Second));
    }
}
