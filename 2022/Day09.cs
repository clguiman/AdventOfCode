using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2022
{
    public class Day09
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(13, Solve(ParseInput(new[] {
                "R 4",
                "U 4",
                "L 3",
                "D 1",
                "R 4",
                "D 1",
                "L 5",
                "R 2"
            }), 2));
        }
        [Fact]
        public void Test2()
        {
            Assert.Equal(6563, Solve(ParseInput(File.ReadAllLines("input/day09.txt")), 2));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(1, Solve(ParseInput(new[] {
                "R 4",
                "U 4",
                "L 3",
                "D 1",
                "R 4",
                "D 1",
                "L 5",
                "R 2"
            }), 10));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(36, Solve(ParseInput(new[] {
                "R 5",
                "U 8",
                "L 8",
                "D 3",
                "R 17",
                "D 10",
                "L 25",
                "U 20"
            }), 10));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(2653, Solve(ParseInput(File.ReadAllLines("input/day09.txt")), 10));
        }

        private static int Solve(IEnumerable<((int x, int y) direction, int steps)> motions, int ropeLength)
        {
            HashSet<(int, int)> uniquePositions = new();
            var rope = Enumerable.Range(0, ropeLength).Select(_ => (x: 0, y: 0)).ToArray();
            foreach (var motion in motions)
            {
                for (var step = 0; step < motion.steps; step++)
                {
                    rope[0].x += motion.direction.x;
                    rope[0].y += motion.direction.y;
                    for (var idx = 1; idx < ropeLength; idx++)
                    {
                        rope[idx] = ComputeFollowerPosition(rope[idx - 1], rope[idx]);
                    }
                    uniquePositions.Add(rope[ropeLength - 1]);
                }
            }
            return uniquePositions.Count;
        }

        private static (int x, int y) ComputeFollowerPosition((int x, int y) head, (int x, int y) tail)
        {
            if (head.x != tail.x || head.y != tail.y)
            {
                if ((head.x != tail.x && head.y == tail.y) || (head.x == tail.x && head.y != tail.y))
                {
                    // follow orthogonally
                    var newTailLocation = Grid2D<int>.GenerateAdjacentOrthogonalLocations(head.x, head.y).Intersect(Grid2D<int>.GenerateAdjacentOrthogonalLocations(tail.x, tail.y)).FirstOrDefault((x: int.MinValue, y: int.MinValue));
                    if (newTailLocation.x != int.MinValue)
                    {
                        return newTailLocation;
                    }
                }
                else if (!Grid2D<int>.GenerateAllAdjacentLocations(head.x, head.y).Any(t => t == tail))
                {
                    // follow diagonally
                    var newTailLocation = Grid2D<int>.GenerateAllAdjacentLocations(head.x, head.y).Intersect(Grid2D<int>.GenerateAllAdjacentLocations(tail.x, tail.y)).First();
                    return newTailLocation;
                }
            }
            return tail;
        }

        private static IEnumerable<((int x, int y) direction, int steps)> ParseInput(IEnumerable<string> input)
        {
            foreach (var line in input)
            {
                var tokens = line.Split(' ');
                var steps = int.Parse(tokens[1]);
                switch (tokens[0][0])
                {
                    case 'U':
                        {
                            yield return ((0, -1), steps);
                            break;
                        }
                    case 'D':
                        {
                            yield return ((0, 1), steps);
                            break;
                        }
                    case 'R':
                        {
                            yield return ((1, 0), steps);
                            break;
                        }
                    case 'L':
                        {
                            yield return ((-1, 0), steps);
                            break;
                        }
                    default: throw new ArgumentException(line);
                }
            }
        }
    }
}