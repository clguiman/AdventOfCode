using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2022
{
    public class Day12
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(31, Part1(new[] {
                "Sabqponm",
                "abcryxxl",
                "accszExk",
                "acctuvwj",
                "abdefghi"
            }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(472, Part1(File.ReadAllLines("input/day12.txt")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(29, Part2(new[] {
                "Sabqponm",
                "abcryxxl",
                "accszExk",
                "acctuvwj",
                "abdefghi"
            }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(465, Part2(File.ReadAllLines("input/day12.txt")));
        }

        private static long Part1(IEnumerable<string> input)
        {
            var (map, startPos, endPos) = ParseInput(input);
            return map.ComputeWalkCost(startPos,
                   t =>
                   {
                       return (t.possibleAdjacent.item - t.current.item) <= 1;
                   },
                   t => { },
                   (_, _) => 1,
                   useOnlyOrthogonalWalking: true
               ).At(endPos.x, endPos.y);
        }

        private static long Part2(IEnumerable<string> input)
        {
            var (map, _, endPos) = ParseInput(input);
            var costMap = map.ComputeWalkCost(endPos,
                t =>
                {
                    return (t.current.item - t.possibleAdjacent.item) <= 1;
                },
                t => { },
                (_, _) => 1,
                    useOnlyOrthogonalWalking: true
                );

            return map.Enumerate().Where(t => t.value == 'a').Select(t => costMap.At(t.x, t.y)).Min();
        }

        private static (Grid2D<char> map, (int x, int y) startPos, (int x, int y) endPos) ParseInput(IEnumerable<string> input)
        {
            var map = new Grid2D<char>(input.Select(x => x.ToArray()));

            var start = map.Enumerate().FirstOrDefault(t => t.value == 'S');
            var end = map.Enumerate().FirstOrDefault(t => t.value == 'E');

            map.AtRef(start.x, start.y) = 'a';
            map.AtRef(end.x, end.y) = 'z';

            return (map, (start.x, start.y), (end.x, end.y));
        }
    }
}