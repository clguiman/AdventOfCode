using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day05
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(5, Part1(new[] {
                "0,9 -> 5,9",
                "8,0 -> 0,8",
                "9,4 -> 3,4",
                "2,2 -> 2,1",
                "7,0 -> 7,4",
                "6,4 -> 2,0",
                "0,9 -> 2,9",
                "3,4 -> 1,4",
                "0,0 -> 8,8",
                "5,5 -> 8,2"
            }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(6225, Part1(File.ReadAllLines("input/day05.txt")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(12, Part2(new[] {
                "0,9 -> 5,9",
                "8,0 -> 0,8",
                "9,4 -> 3,4",
                "2,2 -> 2,1",
                "7,0 -> 7,4",
                "6,4 -> 2,0",
                "0,9 -> 2,9",
                "3,4 -> 1,4",
                "0,0 -> 8,8",
                "5,5 -> 8,2"
            }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(22116, Part2(File.ReadAllLines("input/day05.txt")));
        }

        private static int Part1(string[] input) => ConstructDiagram(ParseInput(input)
                    .Where(i => i.Item1.X == i.Item2.X || i.Item1.Y == i.Item2.Y))
                .SelectMany(x => x).Count(x => x > 1);

        private static int Part2(string[] input) => ConstructDiagram(ParseInput(input)).SelectMany(x => x).Count(x => x > 1);

        private static int[][] ConstructDiagram(IEnumerable<(Point, Point)> lines)
        {
            var height = lines.Max(i => i.Item1.Y > i.Item2.Y ? i.Item1.Y : i.Item2.Y) + 1;
            var width = lines.Max(i => i.Item1.X > i.Item2.X ? i.Item1.X : i.Item2.X) + 1;

            var diagram = Enumerable.Range(0, height).Select(_ => Enumerable.Range(0, width).Select(__ => 0).ToArray()).ToArray();

            foreach (var line in lines)
            {
                if (line.Item1.X == line.Item2.X)
                {
                    for (var i = line.Item1.Y; i <= line.Item2.Y; i++)
                    {
                        diagram[i][line.Item1.X]++;
                    }
                    continue;
                }
                if (line.Item1.Y == line.Item2.Y)
                {
                    for (var i = line.Item1.X; i <= line.Item2.X; i++)
                    {
                        diagram[line.Item1.Y][i]++;
                    }
                    continue;
                }
                var j = line.Item1.X;
                if (line.Item1.Y < line.Item2.Y)
                {
                    for (var i = line.Item1.Y; i <= line.Item2.Y; i++)
                    {
                        diagram[i][j]++;
                        j++;
                    }
                    continue;
                }
                else
                {
                    for (var i = line.Item1.Y; i >= line.Item2.Y; i--)
                    {
                        diagram[i][j]++;
                        j++;
                    }
                    continue;
                }
            }
            return diagram;
        }

        private class Point
        {
            public int X;
            public int Y;
            public override string ToString() => $"{X},{Y}";
        }

        private static IEnumerable<(Point, Point)> ParseInput(string[] input)
        {
            return input.Select(l =>
            {
                var points = l.Split("->")
                              .SelectMany(x => x.Trim().Split(',').Select(int.Parse))
                              .Chunk(2)
                              .Select(c => new Point { X = c.First(), Y = c.Last() })
                              .ToArray();

                var a = points[0];
                var b = points[1];
                if (a.X == b.X)
                {
                    return a.Y > b.Y ? (b, a) : (a, b);
                }
                if (a.Y == b.Y)
                {
                    return a.X > b.X ? (b, a) : (a, b);
                }
                if (a.X < b.X)
                {
                    return (a, b);
                }
                else
                {
                    return (b, a);
                }
            });
        }
    }
}
