using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2019
{
    public class Day20
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(23, Solution1(ParseInput(new[] {
"         A           ",
"         A           ",
"  #######.#########  ",
"  #######.........#  ",
"  #######.#######.#  ",
"  #######.#######.#  ",
"  #######.#######.#  ",
"  #####  B    ###.#  ",
"BC...##  C    ###.#  ",
"  ##.##       ###.#  ",
"  ##...DE  F  ###.#  ",
"  #####    G  ###.#  ",
"  #########.#####.#  ",
"DE..#######...###.#  ",
"  #.#########.###.#  ",
"FG..#########.....#  ",
"  ###########.#####  ",
"             Z       ",
"             Z       "
            })));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(58, Solution1(ParseInput(new[] {
"                   A               ",
"                   A               ",
"  #################.#############  ",
"  #.#...#...................#.#.#  ",
"  #.#.#.###.###.###.#########.#.#  ",
"  #.#.#.......#...#.....#.#.#...#  ",
"  #.#########.###.#####.#.#.###.#  ",
"  #.............#.#.....#.......#  ",
"  ###.###########.###.#####.#.#.#  ",
"  #.....#        A   C    #.#.#.#  ",
"  #######        S   P    #####.#  ",
"  #.#...#                 #......VT",
"  #.#.#.#                 #.#####  ",
"  #...#.#               YN....#.#  ",
"  #.###.#                 #####.#  ",
"DI....#.#                 #.....#  ",
"  #####.#                 #.###.#  ",
"ZZ......#               QG....#..AS",
"  ###.###                 #######  ",
"JO..#.#.#                 #.....#  ",
"  #.#.#.#                 ###.#.#  ",
"  #...#..DI             BU....#..LF",
"  #####.#                 #.#####  ",
"YN......#               VT..#....QG",
"  #.###.#                 #.###.#  ",
"  #.#...#                 #.....#  ",
"  ###.###    J L     J    #.#.###  ",
"  #.....#    O F     P    #.#...#  ",
"  #.###.#####.#.#####.#####.###.#  ",
"  #...#.#.#...#.....#.....#.#...#  ",
"  #.#####.###.###.#.#.#########.#  ",
"  #...#.#.....#...#.#.#.#.....#.#  ",
"  #.###.#####.###.###.#.#.#######  ",
"  #.#.........#...#.............#  ",
"  #########.###.###.#############  ",
"           B   J   C               ",
"           U   P   P               "
            })));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(664, Solution1(ParseInput(File.ReadAllLines("input/day20.txt"))));
        }

        private static long Solution1((Grid2D<char> map, HashSet<Portal> portals) input)
        {
            Dictionary<Portal, Grid2D<long>> portalCostMaps = new();
            foreach (var portal in input.portals)
            {
                portalCostMaps.Add(portal,
                    input.map.Clone().ComputeWalkCost((portal.X, portal.Y),
                        shouldWalkPredicate: t => t.possibleAdjacent.item != '#',
                        onWalkNext: _ => { },
                        walkCostFunc: (_, __) => 1,
                        true));
            }
            Graph<Portal, long> graph = new();

            foreach (var originPortal in input.portals)
            {
                foreach (var destPortal in input.portals)
                {
                    if (originPortal.Equals(destPortal))
                    {
                        continue;
                    }

                    if (string.Equals(originPortal.Name, destPortal.Name, StringComparison.Ordinal))
                    {
                        graph.AddEdge(originPortal, destPortal, 1, _ => false);
                        continue;
                    }

                    graph.AddEdge(originPortal, destPortal, portalCostMaps[originPortal].At(destPortal.X, destPortal.Y), _ => false);
                }
            }

            var aa = input.portals.First(p => string.Equals(p.Name, "AA", StringComparison.Ordinal));

            var minDistances = graph.Dijkstra(aa, x => x);
            return minDistances.First(x => string.Equals(x.Key.Name, "ZZ", StringComparison.Ordinal)).Value;
        }

        private static (Grid2D<char> map, HashSet<Portal> portals) ParseInput(IEnumerable<string> rawInput)
        {
            var input = rawInput.Select(line => line.ToCharArray()).ToArray();
            Grid2D<char> map = new(input
                .Skip(2).SkipLast(2)
                .Select(line => line.Skip(2).SkipLast(2))
                .Select(line => line.Select(c => c != '.' && c != '#' ? '#' : c))
                );
            HashSet<Portal> portals = new();
            // vertical portals
            for (var lineIdx = 0; lineIdx < input.Length - 1; lineIdx++)
            {
                var line = input[lineIdx];
                bool foundPortal = false;
                for (var charIdx = 0; charIdx < line.Length; charIdx++)
                {
                    if (line[charIdx] < 'A' || line[charIdx] > 'Z')
                    {
                        continue;
                    }
                    if (input[lineIdx + 1][charIdx] < 'A' || input[lineIdx + 1][charIdx] > 'Z')
                    {
                        continue;
                    }
                    foundPortal = true;
                    int y;
                    if (lineIdx < input.Length - 2 && input[lineIdx + 2][charIdx] == '.')
                    {
                        y = lineIdx;
                    }
                    else
                    {
                        if (input[lineIdx - 1][charIdx] != '.')
                        {
                            throw new Exception("invalid map!");
                        }
                        else
                        {
                            y = lineIdx - 3;
                        }
                    }
                    portals.Add(new($"{line[charIdx]}{input[lineIdx + 1][charIdx]}", charIdx - 2, y));
                }
                if (foundPortal)
                {
                    lineIdx++;
                }
            }

            // horizontal portals
            for (var colIdx = 0; colIdx < input[0].Length - 1; colIdx++)
            {
                bool foundPortal = false;
                for (var charIdx = 2; charIdx < input.Length - 2; charIdx++)
                {
                    if (input[charIdx][colIdx] < 'A' || input[charIdx][colIdx] > 'Z')
                    {
                        continue;
                    }
                    if (input[charIdx][colIdx + 1] < 'A' || input[charIdx][colIdx + 1] > 'Z')
                    {
                        continue;
                    }
                    int x;
                    if (colIdx < input[0].Length - 2 && input[charIdx][colIdx + 2] == '.')
                    {
                        x = colIdx;
                    }
                    else
                    {
                        if (input[charIdx][colIdx - 1] != '.')
                        {
                            throw new Exception("invalid map!");
                        }
                        else
                        {
                            x = colIdx - 3;
                        }
                    }
                    portals.Add(new($"{input[charIdx][colIdx]}{input[charIdx][colIdx + 1]}", x, charIdx - 2));
                }
                if (foundPortal)
                {
                    colIdx++;
                }
            }

            return (map, portals);
        }

        private class Portal
        {
            public Portal(string name, int x, int y)
            {
                Name = name;
                X = x;
                Y = y;
            }

            public string Name { get; }
            public int X { get; }
            public int Y { get; }

            public override bool Equals(object obj) => obj is Portal p &&
                p.X == X && p.Y == Y && string.Equals(p.Name, Name, StringComparison.Ordinal);

            public override int GetHashCode() => HashCode.Combine(Name, X, Y);

            public override string ToString() => $"{Name} {{{X},{Y}}}";
        }
    }
}
