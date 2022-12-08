using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2022
{
    public class Day08
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(21, Part1(ParseInput(new[] {
                "30373",
                "25512",
                "65332",
                "33549",
                "35390"
            })));
        }
        [Fact]
        public void Test2()
        {
            Assert.Equal(1698, Part1(ParseInput(File.ReadAllLines("input/day08.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(8, Part2(ParseInput(new[] {
                "30373",
                "25512",
                "65332",
                "33549",
                "35390"
            })));
        }
        [Fact]
        public void Test4()
        {
            Assert.Equal(672280, Part2(ParseInput(File.ReadAllLines("input/day08.txt"))));
        }

        private static int Part1(Grid2D<int> map) => map
            .Where(tree => DirectionVectors.Any(vec =>
                {
                    var distance = ComputeVisibilityDistance(map, tree.x, tree.y, vec);
                    if (distance == 0)
                    {
                        return true;
                    }
                    if (vec.x == -1 && tree.x == distance && map.At(0, tree.y) < tree.value)
                    {
                        return true;
                    }
                    if (vec.y == -1 && tree.y == distance && map.At(tree.x, 0) < tree.value)
                    {
                        return true;
                    }
                    if (vec.x == 1 && (map.Width - tree.x - 1) == distance && map.At(map.Width - 1, tree.y) < tree.value)
                    {
                        return true;
                    }
                    if (vec.y == 1 && (map.Height - tree.y - 1) == distance && map.At(tree.x, map.Height - 1) < tree.value)
                    {
                        return true;
                    }
                    return false;
                }))
            .Count();

        private static int Part2(Grid2D<int> map) => map
            .Select(tree =>
                    DirectionVectors.Select(vec => ComputeVisibilityDistance(map, tree.x, tree.y, vec))
                                    .Aggregate((x, y) => x * y))
            .Where(x => x != 0).Max();

        private static readonly (int x, int y)[] DirectionVectors = new (int x, int y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

        private static int ComputeVisibilityDistance(Grid2D<int> map, int xStart, int yStart, (int x, int y) directionVec)
        {
            var startValue = map.At(xStart, yStart);
            var x = xStart + directionVec.x;
            var y = yStart + directionVec.y;
            if (x < 0 || x >= map.Width || y < 0 || y >= map.Height)
            {
                return 0;
            }
            var distance = 0;
            while (x >= 0 && x < map.Width && y >= 0 && y < map.Height)
            {
                distance++;
                if (map.At(x, y) >= startValue)
                {
                    break;
                }
                x += directionVec.x;
                y += directionVec.y;
            }
            return distance;
        }

        private static Grid2D<int> ParseInput(IEnumerable<string> input) => new(input.Select(line => line.ToArray().Select(c => c - '0')));
    }
}