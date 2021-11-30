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
            var input = new[] { 1721, 979, 366, 299, 675, 1456 };
            Assert.Equal(0, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(0, Part1(File.ReadAllLines("input/day01.txt").Select(int.Parse)));
        }

        private static int Part1(IEnumerable<int> input)
        {
            return 0;
        }
    }
}
