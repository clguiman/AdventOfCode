using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2021
{
    public class Day09
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(15, Part1(new(new[] {
                "2199943210",
                "3987894921",
                "9856789892",
                "8767896789",
                "9899965678"
            }.Select(line => line.Select(c => (int)c - (int)'0')))));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(575, Part1(new(File.ReadAllLines("input/day09.txt").Select(line => line.Select(c => (int)c - (int)'0')))));
        }


        [Fact]
        public void Test3()
        {
            Assert.Equal(1134, Part2(new(new[] {
                "2199943210",
                "3987894921",
                "9856789892",
                "8767896789",
                "9899965678"
            }.Select(line => line.Select(c => (int)c - (int)'0')))));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(1019700, Part2(new(File.ReadAllLines("input/day09.txt").Select(line => line.Select(c => (int)c - (int)'0')))));
        }

        private static int Part1(Grid2D<int> input) => input
            .EnumerateWithAdjacentValues()
            .Where(x => x.adjacentValues.All(adjacent => adjacent > x.currentValue))
            .Select(x => x.currentValue + 1)
            .Sum();

        private static int Part2(Grid2D<int> input) => FindLowPoints(input)
            .Select(lowPoint => input.Clone()
                .BFS(lowPoint,
                    shouldWalkPredicate: (t) => t.possibleAdjacentItem != 9 && t.possibleAdjacentItem > t.currentItem,
                    markVisitedFunc: (_) => int.MinValue)
                .Enumerate()
                .Count(x => x == int.MinValue))
            .OrderByDescending(x => x)
            .Take(3)
            .Aggregate((a, b) => a * b);

        private static IEnumerable<(int x, int y)> FindLowPoints(Grid2D<int> input)
        {
            for (var i = 0; i < input.Height; i++)
            {
                for (var j = 0; j < input.Width; j++)
                {
                    bool isLowPoint = true;
                    foreach (var (x, y) in input.GetAdjacentLocations(j, i))
                    {
                        if (input.At(x, y) <= input.At(j, i))
                        {
                            isLowPoint = false;
                            break;
                        }
                    }
                    if (isLowPoint)
                    {
                        yield return (j, i);
                    }
                }
            }
        }
    }
}
