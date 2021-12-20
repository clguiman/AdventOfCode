using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2018
{
    public class Day03
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(4, Part1(ParseInput(new[] {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2"
            })));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(100595, Part1(ParseInput(File.ReadAllLines("input/day03.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(3, Part2(ParseInput(new[] {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2"
            })));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(415, Part2(ParseInput(File.ReadAllLines("input/day03.txt"))));
        }

        private static long Part1(IEnumerable<Rect> input) => Simulate(input).Count(x => x > 1);

        private static int Part2(IEnumerable<Rect> input)
        {
            var grid = Simulate(input);

            var idx = 0;
            foreach (var rect in input)
            {
                idx++;
                bool found = false;
                for (var y = rect.Y1; y <= rect.Y2; y++)
                {
                    for (var x = rect.X1; x <= rect.X2; x++)
                    {
                        found = (grid.At(x, y) != 1);
                        if (found)
                        {
                            y = rect.Y2;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    return idx;
                }
            }
            return 0;
        }

        private static Grid2D<int> Simulate(IEnumerable<Rect> input)
        {
            Grid2D<int> grid = new(1001, 1001);
            foreach (var rect in input)
            {
                for (var y = rect.Y1; y <= rect.Y2; y++)
                {
                    for (var x = rect.X1; x <= rect.X2; x++)
                    {
                        grid.AtRef(x, y)++;
                    }
                }
            }
            return grid;
        }

        private static List<Rect> ParseInput(IEnumerable<string> input)
        {
            List<Rect> ret = new();
            foreach (var line in input)
            {
                var s = line.Split('@')[1].Split(':').Select(x => x.Trim()).ToArray();
                var top = s[0].Split(',').Select(int.Parse).ToArray();
                var dimensions = s[1].Split('x').Select(int.Parse).ToArray();
                ret.Add(new(top[0], top[1], dimensions[0], dimensions[1]));
            }
            return ret;
        }

        private class Rect
        {
            public Rect(int leftX, int leftY, int width, int height)
            {
                X1 = leftX;
                Y1 = leftY;
                X2 = leftX + width - 1;
                Y2 = leftY + height - 1;
            }

            public int X1 { get; }
            public int Y1 { get; }
            public int X2 { get; }
            public int Y2 { get; }

            public override string ToString() => $"({X1},{Y1})({X2},{Y2})";
        }
    }
}
