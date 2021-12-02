using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day02
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(150, Part1(new[] { "forward 5", "down 5", "forward 8", "up 3", "down 8", "forward 2" }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(1727835, Part1(File.ReadAllLines("input/day02.txt")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(900, Part2(new[] { "forward 5", "down 5", "forward 8", "up 3", "down 8", "forward 2" }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(1544000595, Part2(File.ReadAllLines("input/day02.txt")));
        }

        static IEnumerable<(string command, int unit)> ParseCommands(IEnumerable<string> commands) => commands.Select(c =>
            {
                var s = c.Split(' ');
                return (s[0], int.Parse(s[1]));
            });


        private static int Part1(IEnumerable<string> input)
        {
            int posX = 0;
            int depth = 0;
            foreach(var (command, unit) in ParseCommands(input))
            {
                switch (command)
                {
                    case "forward":
                        posX += unit;
                        break;
                    case "up":
                        depth -= unit;
                        break;
                    case "down":
                        depth += unit;
                        break;
                }
            }
            return posX * depth;
        }

        private static int Part2(IEnumerable<string> input)
        {
            int posX = 0;
            int depth = 0;
            int aim = 0;
            foreach (var (command, unit) in ParseCommands(input))
            {
                switch (command)
                {
                    case "forward":
                        posX += unit;
                        depth += aim * unit;
                        break;
                    case "up":
                        aim -= unit;
                        break;
                    case "down":
                        aim += unit;
                        break;
                }
            }
            return posX * depth;
        }
    }
}
