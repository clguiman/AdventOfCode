using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2022
{
    public class Day01
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { 1 };
            Assert.Equal(1, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(1, Part1(File.ReadAllLines("input/day01.txt").Select(int.Parse)));
        }

        private static int Part1(IEnumerable<int> input) => input.Sum();
    }
}
