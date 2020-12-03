using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day03
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(7, Part1(ParseTrees(
@"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#").ToArray(), 3, 1));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(153, Part1(ParseTrees(File.ReadAllText("input/day03.txt")).ToArray(), 3, 1));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(336, Part2(
@"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#"));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(2421944712, Part2(File.ReadAllText("input/day03.txt")));
        }

        private static int Part1(IEnumerable<Tree> trees, int slopeRight, int slopeDown)
        {
            var bottomY = trees.Last().Y;
            var rightX = trees.Max(t => t.X);
            var cur = (X: 0, Y: 0);
            var count = 0;
            while (cur.Y < bottomY)
            {
                cur.X += slopeRight;
                cur.X %= rightX + 1;
                cur.Y += slopeDown;
                if (trees.Where(t => t.X == cur.X && t.Y == cur.Y).Any())
                {
                    count++;
                }
            }
            return count;
        }

        private static long Part2(string map)
        {
            var slopes = new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
            var trees = ParseTrees(map).ToArray();
            return slopes.Select(s => (long)Part1(trees, s.Item1, s.Item2)).Aggregate((x, y) => x * y);
        }

        private static IEnumerable<Tree> ParseTrees(string map)
        {
            var y = 0;
            foreach (var line in map.Split('\n'))
            {
                foreach (var tree in Enumerable
                    .Range(0, line.Length)
                    .Where(x => line[x] == '#')
                    .Select(x => new Tree() { X = x, Y = y }))
                {
                    yield return tree;
                }
                y++;
            }
        }

        private struct Tree
        {
            public int X { get; init; }
            public int Y { get; init; }
        }
    }
}
