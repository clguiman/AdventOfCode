using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day09
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(15, Part1(new[] {
                "2199943210",
                "3987894921",
                "9856789892",
                "8767896789",
                "9899965678"
            }.Select(line => line.Select(c => (int)c - (int)'0').ToArray()).ToArray()));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(575, Part1(File.ReadAllLines("input/day09.txt").Select(line => line.Select(c => (int)c - (int)'0').ToArray()).ToArray()));
        }


        [Fact]
        public void Test3()
        {
            Assert.Equal(1134, Part2(new[] {
                "2199943210",
                "3987894921",
                "9856789892",
                "8767896789",
                "9899965678"
            }.Select(line => line.Select(c => (int)c - (int)'0').ToArray()).ToArray()));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(1019700, Part2(File.ReadAllLines("input/day09.txt").Select(line => line.Select(c => (int)c - (int)'0').ToArray()).ToArray()));
        }

        private static int Part1(int[][] input) => FindLowPoints(input).Select(t => input[t.y][t.x] + 1).Sum();

        private static int Part2(int[][] input)
        {
            var basinSizes = new List<int>();
            foreach (var (lowPointX, lowPointY) in FindLowPoints(input))
            {
                var size = 1;
                var inputCopy = input.Select(l => l.ToArray()).ToArray();
                List<(int x, int y)> locationsToVisit = new() { (lowPointX, lowPointY) };

                while (locationsToVisit.Count > 0)
                {
                    var newLocations = new List<(int x, int y)>();
                    foreach (var (curX, curY) in locationsToVisit)
                    {
                        newLocations.AddRange(GetAdjacentBasinLocations(inputCopy, curX, curY));
                        inputCopy[curY][curX] = int.MinValue;
                    }
                    locationsToVisit = newLocations.Distinct().ToList();
                    size += locationsToVisit.Count;
                }
                basinSizes.Add(size);
            }
            return basinSizes.OrderByDescending(x => x).Take(3).Aggregate((a, b) => a * b);
        }

        private static IEnumerable<(int x, int y)> GetAdjacentBasinLocations(int[][] input, int locationX, int locationY) =>
            GetAdjacentLocations(input, locationX, locationY).Where(t => input[t.y][t.x] != 9 && input[t.y][t.x] > input[locationY][locationX]);

        private static IEnumerable<(int x, int y)> FindLowPoints(int[][] input)
        {
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[0].Length; j++)
                {
                    bool isLowPoint = true;
                    foreach (var (x, y) in GetAdjacentLocations(input, j, i))
                    {
                        if (input[y][x] <= input[i][j])
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

        private static IEnumerable<(int x, int y)> GetAdjacentLocations(int[][] input, int locationX, int locationY)
        {
            if (locationY > 0)
            {
                yield return (locationX, locationY - 1);
            }
            if (locationY < input.Length - 1)
            {
                yield return (locationX, locationY + 1);
            }
            if (locationX > 0)
            {
                yield return (locationX - 1, locationY);
            }
            if (locationX < input[0].Length - 1)
            {
                yield return (locationX + 1, locationY);
            }
        }
    }
}
