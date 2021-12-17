using System;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day17
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(45, Part1(ParseInput("target area: x=20..30, y=-10..-5")));
            Assert.Equal(66, Part1(ParseInput("target area: x=352..377, y=-49..-30")));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(5671, Part1(ParseInput(File.ReadAllText("input/day17.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(112, Part2(ParseInput("target area: x=20..30, y=-10..-5")));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(4556, Part2(ParseInput(File.ReadAllText("input/day17.txt"))));
        }

        private static long Part1(Area targetArea) =>
            Enumerable.Range((int)Math.Sqrt(2 * targetArea.X1),
                             (int)Math.Sqrt(2 * targetArea.X2) - (int)Math.Sqrt(2 * targetArea.X1) + 1)
                .SelectMany(x => Enumerable.Range(0, Math.Abs(targetArea.Y1))
                    .Select(y => RunSimulation(x, y, targetArea)))
                .Max();

        private static long Part2(Area targetArea) =>
            Enumerable.Range((int)Math.Sqrt(2 * targetArea.X1), targetArea.X2 - (int)Math.Sqrt(2 * targetArea.X1) + 1)
                .SelectMany(x => Enumerable.Range(targetArea.Y1, Math.Abs(targetArea.Y1) - targetArea.Y1)
                    .Select(y => RunSimulation(x, y, targetArea)))
                .Count(x => x != long.MinValue);

        private static long RunSimulation(int vX, int vY, Area targetArea)
        {
            var x = 0;
            var y = 0;
            var highestY = y;
            while (x <= targetArea.X2 && y >= targetArea.Y1)
            {
                if (y > highestY)
                {
                    highestY = y;
                }

                x += vX;
                y += vY;
                if (targetArea.Contains(x, y))
                {
                    return highestY;
                }

                if (vX < 0)
                {
                    vX++;
                }
                else if (vX > 0)
                {
                    vX--;
                }
                vY--;
            }
            return long.MinValue;
        }

        private static Area ParseInput(string input)
        {
            var s = input.Substring(13).Split(',');
            var x = s[0].Substring(2).Split("..");
            var y = s[1].Substring(3).Split("..");
            return new Area() { X1 = int.Parse(x[0]), X2 = int.Parse(x[1]), Y1 = int.Parse(y[0]), Y2 = int.Parse(y[1]) };
        }

        private struct Area
        {
            public int X1;
            public int Y1;
            public int X2;
            public int Y2;

            public int Height => Y2 - Y1;
            public bool Contains(int x, int y) => x >= X1 && x <= X2 && y >= Y1 && y <= Y2;
        }
    }
}

