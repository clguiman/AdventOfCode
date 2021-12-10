using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
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
                    .Count(x => x > 1);

        private static int Part2(string[] input) => ConstructDiagram(ParseInput(input)).Count(x => x > 1);

        private static Grid2D<int> ConstructDiagram(IEnumerable<(Coordinates2D, Coordinates2D)> lines)
        {
            var height = lines.Max(i => i.Item1.Y > i.Item2.Y ? i.Item1.Y : i.Item2.Y) + 1;
            var width = lines.Max(i => i.Item1.X > i.Item2.X ? i.Item1.X : i.Item2.X) + 1;

            var diagram = new Grid2D<int>(Enumerable.Range(0, height).Select(_ => Enumerable.Range(0, width).Select(__ => 0)));

            foreach (var line in lines)
            {
                if (line.Item1.X == line.Item2.X)
                {
                    for (var i = line.Item1.Y; i <= line.Item2.Y; i++)
                    {
                        diagram.AtRef(line.Item1.X, i)++;
                    }
                    continue;
                }
                if (line.Item1.Y == line.Item2.Y)
                {
                    for (var i = line.Item1.X; i <= line.Item2.X; i++)
                    {
                        diagram.AtRef(i, line.Item1.Y)++;
                    }
                    continue;
                }
                var x = line.Item1.X;
                if (line.Item1.Y < line.Item2.Y)
                {
                    for (var y = line.Item1.Y; y <= line.Item2.Y; y++)
                    {
                        diagram.AtRef(x, y)++;
                        x++;
                    }
                    continue;
                }
                else
                {
                    for (var y = line.Item1.Y; y >= line.Item2.Y; y--)
                    {
                        diagram.AtRef(x, y)++;
                        x++;
                    }
                    continue;
                }
            }
            return diagram;
        }

        private static IEnumerable<(Coordinates2D, Coordinates2D)> ParseInput(string[] input)
        {
            return input.Select(l =>
            {
                var points = l.Split("->")
                              .SelectMany(x => x.Trim().Split(',').Select(int.Parse))
                              .Chunk(2)
                              .Select(c => new Coordinates2D { X = c.First(), Y = c.Last() })
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
