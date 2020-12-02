using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day02
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { "1-3 a: abcde", "1-3 b: cdefg", "2-9 c: ccccccccc" };
            Assert.Equal(2, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(580, Part1(File.ReadLines("input/day02.txt")));
        }

        [Fact]
        public void Test3()
        {
            var input = new[] { "1-3 a: abcde", "1-3 b: cdefg", "2-9 c: ccccccccc" };
            Assert.Equal(1, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(611, Part2(File.ReadLines("input/day02.txt")));
        }

        private static IEnumerable<(int min, int max, char letter, string password)> ParseInput(IEnumerable<string> input) =>
            input.Select(str =>
            {
                var tokens = str.Split(' ');
                var limits = tokens[0].Split('-');
                return (min: int.Parse(limits[0]), max: int.Parse(limits[1]), letter: tokens[1][0], password: tokens[2]);
            });

        private static int Part1(IEnumerable<string> input) =>
            ParseInput(input).Count(item =>
            {
                var cnt = item.password.Count(c => c == item.letter);
                return cnt >= item.min && cnt <= item.max;
            });

        private static int Part2(IEnumerable<string> input) =>
            ParseInput(input).Count(item =>
            {
                return 1 == new[] { item.min, item.max }
                    .Count(x => item.password.Length >= x && item.password[x - 1] == item.letter);
            });
    }

}
