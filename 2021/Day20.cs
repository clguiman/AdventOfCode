using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2021
{
    public class Day20
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(35, Solve(new[] {
"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..##"+
"#..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###"+
".######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#."+
".#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#....."+
".#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.."+
"...####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#....."+
"..##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#",
"",
"#..#.",
"#....",
"##..#",
"..#..",
"..###"
            }, 2));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(5663, Solve(File.ReadAllLines("input/day20.txt"), 2));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(3351, Solve(new[] {
"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..##"+
"#..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###"+
".######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#."+
".#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#....."+
".#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.."+
"...####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#....."+
"..##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#",
"",
"#..#.",
"#....",
"##..#",
"..#..",
"..###"
            }, 50));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(19638, Solve(File.ReadAllLines("input/day20.txt"), 50));
        }

        private static long Solve(string[] input, int steps)
        {
            var paddingChar = '.';
            var map = AddPadding(new(input.Skip(2).Select(line => line.ToCharArray())), paddingChar);
            var enhancementData = input[0];

            for (var idx = 0; idx < steps; idx++)
            {
                var newMap = map.Clone() as Grid2D<char>;
                for (var y = 0; y < map.Height; y++)
                {
                    for (var x = 0; x < map.Width; x++)
                    {
                        var newItem = enhancementData[Get3x3Square(map, x, y, paddingChar).Select(t => t == '#' ? 1 : 0).Aggregate((a, b) => a * 2 + b)];
                        newMap.SetAt(newItem, x, y);
                    }
                }
                if (enhancementData[0] != '.')
                {
                    paddingChar = idx % 2 == 1 ? enhancementData[511] : enhancementData[0];
                }
                map = AddPadding(newMap, paddingChar);
            }
            return map.Count(x => x == '#');
        }

        private static Grid2D<char> AddPadding(Grid2D<char> map, char paddingChar = '.', int paddingSize = 1)
        {
            var emptyRow = Enumerable.Range(0, map.Width + paddingSize * 2).Select(_ => paddingChar);
            var rowPadding = Enumerable.Range(0, paddingSize).Select(_ => emptyRow);
            var linePadding = Enumerable.Range(0, paddingSize).Select(_ => paddingChar);
            return new(rowPadding.Concat(
                                    map.Rows.Select(row => linePadding.Concat(row).Concat(linePadding)))
                            .Concat(rowPadding));
        }

        private static IEnumerable<char> Get3x3Square(Grid2D<char> map, int locationX, int locationY, char defaultValue)
        {
            return adjacentDeltas.Select(t => (x: t.dX + locationX, y: t.dY + locationY)).Select(t =>
              {
                  if (t.x < 0 || t.x >= map.Width || t.y < 0 || t.y >= map.Height)
                  {
                      return defaultValue;
                  }
                  return map.At(t.x, t.y);
              });
        }

        private static readonly (int dX, int dY)[] adjacentDeltas = Enumerable.Range(-1, 3).SelectMany(y => Enumerable.Range(-1, 3).Select(x => (x, y))).ToArray();
    }
}

