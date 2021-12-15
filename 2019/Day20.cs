using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Assert.Equal(23, Solve(ParseInput(new[] {
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
            }), true));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(58, Solve(ParseInput(new[] {
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
            }), true));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(664, Solve(ParseInput(File.ReadAllLines("input/day20.txt")), true));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(26, Solve(ParseInput(new[] {
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
            }), false));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(396, Solve(ParseInput(new[] {
"             Z L X W       C                 ",
"             Z P Q B       K                 ",
"  ###########.#.#.#.#######.###############  ",
"  #...#.......#.#.......#.#.......#.#.#...#  ",
"  ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###  ",
"  #.#...#.#.#...#.#.#...#...#...#.#.......#  ",
"  #.###.#######.###.###.#.###.###.#.#######  ",
"  #...#.......#.#...#...#.............#...#  ",
"  #.#########.#######.#.#######.#######.###  ",
"  #...#.#    F       R I       Z    #.#.#.#  ",
"  #.###.#    D       E C       H    #.#.#.#  ",
"  #.#...#                           #...#.#  ",
"  #.###.#                           #.###.#  ",
"  #.#....OA                       WB..#.#..ZH",
"  #.###.#                           #.#.#.#  ",
"CJ......#                           #.....#  ",
"  #######                           #######  ",
"  #.#....CK                         #......IC",
"  #.###.#                           #.###.#  ",
"  #.....#                           #...#.#  ",
"  ###.###                           #.#.#.#  ",
"XF....#.#                         RF..#.#.#  ",
"  #####.#                           #######  ",
"  #......CJ                       NM..#...#  ",
"  ###.#.#                           #.###.#  ",
"RE....#.#                           #......RF",
"  ###.###        X   X       L      #.#.#.#  ",
"  #.....#        F   Q       P      #.#.#.#  ",
"  ###.###########.###.#######.#########.###  ",
"  #.....#...#.....#.......#...#.....#.#...#  ",
"  #####.#.###.#######.#######.###.###.#.#.#  ",
"  #.......#.......#.#.#.#.#...#...#...#.#.#  ",
"  #####.###.#####.#.#.#.#.###.###.#.###.###  ",
"  #.......#.....#.#...#...............#...#  ",
"  #############.#.#.###.###################  ",
"               A O F   N                     ",
"               A A D   M                     "
            }), false));
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal(7334, Solve(ParseInput(File.ReadAllLines("input/day20.txt")), false));
        }

        private static long Solve((Grid2D<char> map, HashSet<Portal> portals) input, bool solvePart1)
        {
            var portalCostMaps = GeneratePortalCostMaps(input);
            var graph = solvePart1 ? GeneratePart1Graph(input.portals, portalCostMaps) : GeneratePart2Graph(input.portals, portalCostMaps);

            var aa = input.portals.First(p => string.Equals(p.Name, "AA", StringComparison.Ordinal));
            var zz = input.portals.First(p => string.Equals(p.Name, "ZZ", StringComparison.Ordinal));

            return graph.FindShortestPath(aa, zz, x => x).cost;
        }

        private static Graph<Portal, long> GeneratePart1Graph(HashSet<Portal> portals, ReadOnlyDictionary<Portal, Grid2D<long>> portalCostMaps)
        {
            Graph<Portal, long> graph = new();
            foreach (var originPortal in portals)
            {
                foreach (var destPortal in portals)
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
            return graph;
        }

        private static Graph<Portal, long> GeneratePart2Graph(HashSet<Portal> portals, ReadOnlyDictionary<Portal, Grid2D<long>> portalCostMaps)
        {
            Graph<Portal, long> graph = new();

            for (var level = 0; level < 30; level++)
            {
                foreach (var originPortal in portals.Select(p => p.CloneWithRecurseLevel(level)))
                {

                    var possibleDestinations = portals
                        .Select(p => p.CloneWithRecurseLevel(level))
                        .SelectMany(p => new[] { p, p.CloneInNextRecurseLevel(), p.CloneInPreviousRecurseLevel() })
                        .Where(p => p.RecurseLevel >= 0 && !originPortal.EqualsExceptRecurseLevel(p));

                    foreach (var destPortal in possibleDestinations)
                    {
                        if (string.Equals(originPortal.Name, destPortal.Name, StringComparison.Ordinal))
                        {
                            if (!originPortal.IsInner && destPortal.RecurseLevel != originPortal.RecurseLevel - 1)
                            {
                                continue;
                            }
                            if (originPortal.IsInner && destPortal.RecurseLevel != originPortal.RecurseLevel + 1)
                            {
                                continue;
                            }

                            graph.AddEdge(originPortal, destPortal, 1, _ => false);
                            continue;
                        }

                        if (destPortal.RecurseLevel != originPortal.RecurseLevel)
                        {
                            continue;
                        }
                        var cost = portalCostMaps[originPortal.Level0Clone].At(destPortal.X, destPortal.Y);
                        if (cost == long.MaxValue)
                        {
                            continue;
                        }

                        graph.AddEdge(originPortal, destPortal, cost, _ => false);
                    }
                }
            }
            return graph;
        }

        private static ReadOnlyDictionary<Portal, Grid2D<long>> GeneratePortalCostMaps((Grid2D<char> map, HashSet<Portal> portals) input)
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
            return new(portalCostMaps);
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
                    bool isInner = (lineIdx != 0 && lineIdx != input.Length - 2);
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
                    portals.Add(new($"{line[charIdx]}{input[lineIdx + 1][charIdx]}", charIdx - 2, y, isInner));
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
                    bool isInner = (colIdx != 0 && colIdx != input[0].Length - 2);
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
                    portals.Add(new($"{input[charIdx][colIdx]}{input[charIdx][colIdx + 1]}", x, charIdx - 2, isInner));
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
            public Portal(string name, int x, int y, bool isInner) : this(name, x, y, isInner, 0) { }

            public Portal CloneInNextRecurseLevel() => new(this);

            public Portal CloneInPreviousRecurseLevel() => new(Name, X, Y, IsInner, RecurseLevel - 1);

            public Portal CloneWithRecurseLevel(int level) => new(Name, X, Y, IsInner, level);

            public Portal Level0Clone => new(Name, X, Y, IsInner, 0);

            public string Name { get; }
            public int X { get; }
            public int Y { get; }
            public bool IsInner { get; }
            public int RecurseLevel { get; }


            public bool EqualsExceptRecurseLevel(Portal p) =>
                p.X == X && p.Y == Y && p.IsInner == IsInner &&
                string.Equals(p.Name, Name, StringComparison.Ordinal);

            public override bool Equals(object obj) => obj is Portal p &&
                p.X == X && p.Y == Y && p.RecurseLevel == RecurseLevel && p.IsInner == IsInner &&
                string.Equals(p.Name, Name, StringComparison.Ordinal);

            public override int GetHashCode() => HashCode.Combine(Name, X, Y, IsInner, RecurseLevel);

            public override string ToString() => $"{Name} {{{X},{Y}}} [{RecurseLevel}] {IsInner}";

            private Portal(Portal p)
            {
                Name = p.Name;
                X = p.X;
                Y = p.Y;
                IsInner = p.IsInner;
                RecurseLevel = p.RecurseLevel + 1;
            }

            private Portal(string name, int x, int y, bool isInner, int recurseLevel)
            {
                Name = name;
                X = x;
                Y = y;
                IsInner = isInner;
                RecurseLevel = recurseLevel;
            }
        }
    }
}
