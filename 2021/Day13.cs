using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utils;
using Xunit;

namespace _2021
{
    public class Day13
    {
        [Fact]
        public void Test1()
        {
            var (points, foldings) = ParseInput(new[]
            {
                "6,10",
                "0,14",
                "9,10",
                "0,3",
                "10,4",
                "4,11",
                "6,0",
                "6,12",
                "4,1",
                "0,13",
                "10,12",
                "3,4",
                "3,0",
                "8,4",
                "1,10",
                "2,14",
                "8,10",
                "9,0",
                "",
                "fold along y=7",
                "fold along x=5"
            });
            Assert.Equal(17, Solve(points, foldings.Take(1)).Count(x => x.value));
        }

        [Fact]
        public void Test2()
        {
            var (points, foldings) = ParseInput(File.ReadAllLines("input/day13.txt"));
            Assert.Equal(610, Solve(points, foldings.Take(1)).Count(x => x.value));
        }

        [Fact]
        public void Test3()
        {
            var (points, foldings) = ParseInput(File.ReadAllLines("input/day13.txt"));
            var paper = Solve(points, foldings);

            var sb = new StringBuilder();
            var idx = 0;
            foreach (var item in paper.Where(t => t.x < 39 && t.y < 6).Select(x => x.value ? '#' : '.'))
            {
                if (idx > 0 && idx % 39 == 0)
                {
                    sb.AppendLine();
                }
                sb.Append(item);
                idx++;
            }

            Assert.Equal(
                "###..####.####...##.#..#.###..####.####\r\n" +
                "#..#....#.#.......#.#..#.#..#.#.......#\r\n" +
                "#..#...#..###.....#.####.#..#.###....#.\r\n" +
                "###...#...#.......#.#..#.###..#.....#..\r\n" +
                "#....#....#....#..#.#..#.#.#..#....#...\r\n" +
                "#....####.#.....##..#..#.#..#.#....####"
                , sb.ToString());
        }

        private static Grid2D<bool> Solve(IEnumerable<(int x, int y)> points, IEnumerable<(char direction, int origin)> foldings)
        {
            var width = points.Max(t => t.x) + 1;
            var height = points.Max(t => t.y) + 1;
            var paper = new Grid2D<bool>(width, height);
            foreach (var point in points)
            {
                paper.SetAt(true, point.x, point.y);
            }

            foreach (var fold in foldings)
            {
                if (fold.direction == 'y')
                {
                    var yDest = fold.origin - 1;
                    var ySrc = fold.origin + 1;
                    for (; ySrc < paper.Height && yDest >= 0; ySrc++, yDest--)
                    {
                        for (var x = 0; x < paper.Width; x++)
                        {
                            paper.SetAt(paper.At(x, ySrc) | paper.At(x, yDest), x, yDest);
                            paper.SetAt(false, x, ySrc);
                        }
                    }
                }
                else
                {
                    var xDest = fold.origin - 1;
                    var xSrc = fold.origin + 1;
                    for (; xSrc < paper.Width && xDest >= 0; xSrc++, xDest--)
                    {
                        for (var y = 0; y < paper.Height; y++)
                        {
                            paper.SetAt(paper.At(xSrc, y) | paper.At(xDest, y), xDest, y);
                            paper.SetAt(false, xSrc, y);
                        }
                    }
                }
            }
            return paper;
        }

        private static (List<(int x, int y)> points, List<(char direction, int origin)> foldings) ParseInput(IEnumerable<string> lines)
        {
            List<(int x, int y)> points = new();
            List<(char direction, int size)> foldings = new();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (line.StartsWith("fold"))
                {
                    char direction = ' ';
                    if (line.Contains('x'))
                    {
                        direction = 'x';
                    }
                    else
                    {
                        direction = 'y';
                    }
                    var size = int.Parse(line.Split('=').Last());
                    foldings.Add((direction, size));

                    continue;
                }
                var s = line.Split(",").Select(int.Parse).ToArray();
                points.Add((s[0], s[1]));
            }
            return (points, foldings);
        }
    }
}
