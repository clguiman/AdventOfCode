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

        private static int Part1(Grid2D<int> input) => FindLowPoints(input).Select(t => input.At(t.x, t.y) + 1).Sum();

        private static int Part2(Grid2D<int> input)
        {
            var basinSizes = new List<int>();
            foreach (var (lowPointX, lowPointY) in FindLowPoints(input))
            {
                var size = 1;
                var inputCopy = input.Clone();
                List<(int x, int y)> locationsToVisit = new() { (lowPointX, lowPointY) };

                while (locationsToVisit.Count > 0)
                {
                    var newLocations = new List<(int x, int y)>();
                    foreach (var (curX, curY) in locationsToVisit)
                    {
                        newLocations.AddRange(GetAdjacentBasinLocations(inputCopy, curX, curY));
                        inputCopy.SetAt(int.MinValue, curX, curY);
                    }
                    locationsToVisit = newLocations.Distinct().ToList();
                    size += locationsToVisit.Count;
                }
                basinSizes.Add(size);
            }
            return basinSizes.OrderByDescending(x => x).Take(3).Aggregate((a, b) => a * b);
        }

        private static IEnumerable<(int x, int y)> GetAdjacentBasinLocations(Grid2D<int> input, int locationX, int locationY) =>
            input.GetAdjacentLocations(locationX, locationY).Where(t => input.At(t.x, t.y) != 9 && input.At(t.x, t.y) > input.At(locationX, locationY));

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
