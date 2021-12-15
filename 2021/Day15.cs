using System.IO;
using Utils;
using Xunit;

namespace _2021
{
    public class Day15
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(40, Solve1(new[] {
                "1163751742",
                "1381373672",
                "2136511328",
                "3694931569",
                "7463417111",
                "1319128137",
                "1359912421",
                "3125421639",
                "1293138521",
                "2311944581"
            }.AsDigitGrid()));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(592, Solve1(File.ReadAllLines("input/day15.txt").AsDigitGrid()));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(315, Part2(new[] {
                "1163751742",
                "1381373672",
                "2136511328",
                "3694931569",
                "7463417111",
                "1319128137",
                "1359912421",
                "3125421639",
                "1293138521",
                "2311944581"
            }.AsDigitGrid()));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(2897, Part2(File.ReadAllLines("input/day15.txt").AsDigitGrid()));
        }

        private static long Solve1(Grid2D<int> map) => map
            .ComputeWalkCost((0, 0), _ => true, _ => { }, (_, next) => next, true)
            .At(map.Width - 1, map.Height - 1);

        private static long Part2(Grid2D<int> map)
        {
            var largerMap = new Grid2D<int>(map.Width * 5, map.Height * 5);

            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    largerMap.SetAt(map.At(x, y), x, y);
                }
            }

            for (var yStep = 0; yStep < 5; yStep++)
            {
                for (var xStep = 0; xStep < 5; xStep++)
                {
                    if (xStep == 0 && yStep == 0)
                    {
                        continue;
                    }

                    for (var y = 0; y < map.Height; y++)
                    {
                        for (var x = 0; x < map.Width; x++)
                        {
                            var newVal = map.At(x, y) + xStep + yStep;
                            if (newVal > 9)
                            {
                                newVal %= 9;
                            }
                            largerMap.SetAt(newVal, xStep * map.Width + x, yStep * map.Height + y);
                        }
                    }

                }
            }

            return Solve1(largerMap);
        }
    }
}
