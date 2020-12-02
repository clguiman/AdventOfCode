using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day01
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { 1721, 979, 366, 299, 675, 1456 };
            Assert.Equal(514579, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(889779, Part1(File.ReadAllLines("input/day01.txt").Select(int.Parse)));
        }

        [Fact]
        public void Test3()
        {
            var input = new[] { 1721, 979, 366, 299, 675, 1456 };
            Assert.Equal(241861950, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(76110336, Part2(File.ReadAllLines("input/day01.txt").Select(int.Parse)));
        }

        private static int Part1(IEnumerable<int> input)
        {
            var second = input
                .Select(x => 2020 - x)
                .Where(x => input.Contains(x))
                .First();
            return (2020 - second) * second;
        }

        private static int Part2(IEnumerable<int> input)
        {
            var result = input
                .SelectMany(x => input
                    .Where(y => x + y < 2020)
                    .Select(y => (x, y)))
                .SelectMany(item => input
                    .Select(z => (item.x, item.y, z))
                    .Where(t => t.x + t.y + t.z == 2020)).First();

            return result.x * result.y * result.z;
        }
    }
}
