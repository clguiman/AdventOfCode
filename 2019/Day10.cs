using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Xunit;

namespace _2019
{
    public class Day10
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(2, Solution1(FindAsteroids(
@".#...
..#..
...#.
....#")));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(8, Solution1(FindAsteroids(
@".#..#
.....
#####
....#
...##")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(33, Solution1(FindAsteroids(
@"......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####")));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(35, Solution1(FindAsteroids(
@"#.#...#.#.
.###....#.
.#....#...
##.#.#.#.#
....#.#.#.
.##..###.#
..#...##..
..##....##
......#...
.####.###.")));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(41, Solution1(FindAsteroids(
@".#..#..###
####.###.#
....###.#.
..###.##.#
##.##.#.#.
....###..#
..#.#..#.#
#..#.#.###
.##...##.#
.....#.#..")));
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal(210, Solution1(FindAsteroids(
@".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##").ToArray()));
        }

        [Fact]
        public void Test7()
        {
            Assert.Equal(314, Solution1(FindAsteroids(File.ReadAllText("input/day10.txt"))));
        }

        [Fact]
        public void Test8()
        {
            Assert.Equal(802, Solution2(FindAsteroids(
@".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##"), new Asteroid() { X = 11, Y = 13 }));
        }

        [Fact]
        public void Test9()
        {
            Assert.Equal(1513, Solution2(FindAsteroids(File.ReadAllText("input/day10.txt")), new Asteroid() { X = 27, Y = 19 }));
        }

        private static int Solution1(IEnumerable<Asteroid> asteroids)
        {
            return asteroids
               .Select(a1 => asteroids
                   .Where(a2 => a2.X != a1.X || a2.Y != a1.Y)
                   .Select(a2 => new LineOfSight(a1, a2))
                   .Distinct(new LineOfSightComparer())
               .Count())
               .Max();
        }

        private static int Solution2(IEnumerable<Asteroid> asteroids, Asteroid station)
        {
            var linesOfSight = asteroids
                .Where(a => a.X == station.X && a.Y == station.Y)
                .Select(a1 => asteroids
                   .Where(a2 => a2.X != a1.X || a2.Y != a1.Y)
                   .Select(a2 => new LineOfSight(a1, a2)))
                .First()
                .GroupBy(l => l, new LineOfSightComparer())
                .OrderBy(l => l.Key, new LineOfSightComparer()) // order in clockwise direction
                .Select(l => l.OrderBy(s => s.Length).ToList())
                .ToList();

            int count = 1;
            int idx = 0;

            while (linesOfSight.Count > 0)
            {
                if (linesOfSight[idx].Count == 0)
                {
                    linesOfSight.RemoveAt(idx);
                    continue;
                }
                if (count == 200)
                {
                    return (int)linesOfSight[idx][0].Destination.X * 100 + (int)linesOfSight[idx][0].Destination.Y;
                }
                linesOfSight[idx].RemoveAt(0);
                idx++;
                count++;
            }
            return 0;
        }

        private static IEnumerable<Asteroid> FindAsteroids(string map)
        {
            var y = 0;
            foreach (var line in map.Split('\n'))
            {
                foreach (var asteroid in Enumerable
                    .Range(0, line.Length)
                    .Where(x => line[x] == '#')
                    .Select(x => new Asteroid() { X = x, Y = y }))
                {
                    yield return asteroid;
                }
                y++;
            }
        }

        private struct Asteroid
        {
            public double X { get; init; }
            public double Y { get; init; }

            public static Asteroid Invalid = new() { X = -1, Y = -1 };
        }

        private struct LineOfSight
        {
            public readonly Asteroid Source;
            public readonly Asteroid Destination;
            public readonly double Sin;
            public readonly bool IsRightSideQuadrant;
            public readonly double Length;
            public LineOfSight(Asteroid a1, Asteroid a2)
            {
                Source = a1;
                Destination = a2;
                Length = Math.Sqrt((a1.X - a2.X) * (a1.X - a2.X) + (a1.Y - a2.Y) * (a1.Y - a2.Y));
                Sin = Math.Round((a1.Y - a2.Y) / Length, 9);
                IsRightSideQuadrant = a2.X >= a1.X;
            }
            public enum QuadrantLocation
            {
                // clockwise ordering
                First = 1,
                Second = 4,
                Third = 3,
                Forth = 2
            }

            public QuadrantLocation Quadrant
            {
                get
                {
                    switch (IsRightSideQuadrant)
                    {
                        case true:
                            return Sin >= 0 ? QuadrantLocation.First : QuadrantLocation.Forth;
                        case false:
                            return Sin >= 0 ? QuadrantLocation.Second : QuadrantLocation.Third;
                    }
                }
            }

            public override string ToString()
            {
                return $"({Source.X},{Source.Y})->({Destination.X},{Destination.Y}) right: {IsRightSideQuadrant} sin: {Sin}";
            }

        }

        private class LineOfSightComparer : IEqualityComparer<LineOfSight>, IComparer<LineOfSight>
        {
            public int Compare(LineOfSight x, LineOfSight y)
            {
                // order clockwise
                var q1 = x.Quadrant;
                var q2 = y.Quadrant;
                if (q1 != q2)
                {
                    return q1 > q2 ? 1 : -1;
                }
                if (x.Sin == y.Sin)
                {
                    return 0;
                }

                if (x.IsRightSideQuadrant)
                {
                    return x.Sin > y.Sin ? -1 : 1;
                }
                else
                {
                    return x.Sin > y.Sin ? 1 : -1;
                }
            }

            public bool Equals(LineOfSight x, LineOfSight y)
            {
                return x.IsRightSideQuadrant == y.IsRightSideQuadrant && x.Sin == y.Sin;// no need to use epsilon comparison since the values are already rounded down
            }

            public int GetHashCode([DisallowNull] LineOfSight obj)
            {
                return HashCode.Combine(obj.Sin, obj.IsRightSideQuadrant);
            }
        }
    }
}
