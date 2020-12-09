using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2019
{
    public class Day03
    {
        [Fact]
        public void Test1()
        {
            AssertCollection(new[] {
                new Line() { P1 = new() { X = 0, Y = 0 }, P2 = new() { X = 8, Y = 0 }, StepsUntilHere = 0 },
                new Line() { P1 = new() { X = 8, Y = 0 }, P2 = new() { X = 8, Y = 5 }, StepsUntilHere = 8 },
                new Line() { P1 = new() { X = 8, Y = 5 }, P2 = new() { X = 3, Y = 5 }, StepsUntilHere = 13 },
                new Line() { P1 = new() { X = 3, Y = 5 }, P2 = new() { X = 3, Y = 2 }, StepsUntilHere = 18 },
            }, ParsePath("R8,U5,L5,D3"));
        }

        [Fact]
        public void Test2()
        {
            AssertCollection(new[] {
                new Line() { P1 = new() { X = 0, Y = 0 }, P2 = new() { X = 0, Y = 7 }, StepsUntilHere = 0 },
                new Line() { P1 = new() { X = 0, Y = 7 }, P2 = new() { X = 6, Y = 7 }, StepsUntilHere = 7 },
                new Line() { P1 = new() { X = 6, Y = 7 }, P2 = new() { X = 6, Y = 3 }, StepsUntilHere = 13 },
                new Line() { P1 = new() { X = 6, Y = 3 }, P2 = new() { X = 2, Y = 3 }, StepsUntilHere = 17 },
            }, ParsePath("U7,R6,D4,L4"));
        }

        [Fact]
        public void Test3()
        {
            var line1 = new Line() { P1 = new() { X = 0, Y = 0 }, P2 = new() { X = 8, Y = 0 } };
            var line2 = new Line() { P1 = new() { X = 1, Y = 2 }, P2 = new() { X = 10, Y = 2 } };
            var line3 = new Line() { P1 = new() { X = 0, Y = 0 }, P2 = new() { X = 0, Y = 10 } };
            var line4 = new Line() { P1 = new() { X = 0, Y = 2 }, P2 = new() { X = 0, Y = 5 } };
            var line5 = new Line() { P1 = new() { X = 5, Y = 0 }, P2 = new() { X = 7, Y = 0 } };
            var line6 = new Line() { P1 = new() { X = 5, Y = 0 }, P2 = new() { X = -7, Y = 0 } };

            Assert.Empty(line1.GetIntersections(line2));
            AssertCollection(new[] { new Point() { X = 0, Y = 0 } }, line1.GetIntersections(line3));
            AssertCollection(new[] { new Point() { X = 0, Y = 0 } }, line3.GetIntersections(line1));
            AssertCollection(new[] {
                new Point() { X = 0, Y = 2 },
                new Point() { X = 0, Y = 3 },
                new Point() { X = 0, Y = 4 },
                new Point() { X = 0, Y = 5 } }, line3.GetIntersections(line4));
            AssertCollection(new[] {
                new Point() { X = 0, Y = 2 },
                new Point() { X = 0, Y = 3 },
                new Point() { X = 0, Y = 4 },
                new Point() { X = 0, Y = 5 } }, line4.GetIntersections(line3));
            AssertCollection(new[] {
                new Point() { X = 5, Y = 0 },
                new Point() { X = 6, Y = 0 },
                new Point() { X = 7, Y = 0 } }, line1.GetIntersections(line5));
            AssertCollection(new[] {
                new Point() { X = 5, Y = 0 },
                new Point() { X = 6, Y = 0 },
                new Point() { X = 7, Y = 0 } }, line5.GetIntersections(line1));
            AssertCollection(new[] {
                new Point() { X = 5, Y = 0 },
                new Point() { X = 4, Y = 0 },
                new Point() { X = 3, Y = 0 },
                new Point() { X = 2, Y = 0 },
                new Point() { X = 1, Y = 0 },
                new Point() { X = 0, Y = 0 } }, line1.GetIntersections(line6));
            AssertCollection(new[] {
                new Point() { X = 5, Y = 0 },
                new Point() { X = 4, Y = 0 },
                new Point() { X = 3, Y = 0 },
                new Point() { X = 2, Y = 0 },
                new Point() { X = 1, Y = 0 },
                new Point() { X = 0, Y = 0 } }, line6.GetIntersections(line1));

            AssertCollection(new[] {
                new Point() { X = 3, Y = 3 } },
                new Line() { P1 = new() { X = 6, Y = 3 }, P2 = new() { X = 2, Y = 3 } }.GetIntersections(
                    new Line() { P1 = new() { X = 3, Y = 5 }, P2 = new() { X = 3, Y = 2 } }));

            Assert.Empty(new Line() { P1 = new() { X = 3, Y = 5 }, P2 = new() { X = 3, Y = 2 } }.GetIntersections(
                new Line() { P1 = new() { X = 0, Y = 7 }, P2 = new() { X = 6, Y = 7 } }));
        }

        [Fact]
        public void Test4()
        {
            var path1 = new[] {
                new Line() { P1 = new() { X = 0, Y = 0 }, P2 = new() { X = 8, Y = 0 } },
                new Line() { P1 = new() { X = 8, Y = 0 }, P2 = new() { X = 8, Y = 5 } },
                new Line() { P1 = new() { X = 8, Y = 5 }, P2 = new() { X = 3, Y = 5 } },
                new Line() { P1 = new() { X = 3, Y = 5 }, P2 = new() { X = 3, Y = 2 } },
            };

            var path2 = new[] {
                new Line() { P1 = new() { X = 0, Y = 0 }, P2 = new() { X = 0, Y = 7 } },
                new Line() { P1 = new() { X = 0, Y = 7 }, P2 = new() { X = 6, Y = 7 } },
                new Line() { P1 = new() { X = 6, Y = 7 }, P2 = new() { X = 6, Y = 3 } },
                new Line() { P1 = new() { X = 6, Y = 3 }, P2 = new() { X = 2, Y = 3 } },
            };
            AssertCollection(new[] {
                new Point() { X = 3, Y = 3 },
                new Point() { X = 6, Y = 5 } }, GetPathIntersections(path1, path2));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(6, Solution1("R8,U5,L5,D3", "U7,R6,D4,L4"));
            Assert.Equal(159, Solution1("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83"));
            Assert.Equal(135, Solution1("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"));
        }

        [Fact]
        public void Test6()
        {
            var paths = File.ReadAllLines("input/day3.txt").ToArray();
            Assert.Equal(865, Solution1(paths[0], paths[1]));
        }

        [Fact]
        public void Test7()
        {
            Assert.Equal(30, Solution2("R8,U5,L5,D3", "U7,R6,D4,L4"));
            Assert.Equal(610, Solution2("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83"));
            Assert.Equal(410, Solution2("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"));
        }

        [Fact]
        public void Test8()
        {
            var paths = File.ReadAllLines("input/day3.txt").ToArray();
            Assert.Equal(35038, Solution2(paths[0], paths[1]));
        }

        private void AssertCollection<Type>(IEnumerable<Type> expected, IEnumerable<Type> actual)
        {
            Assert.Equal(expected.Count(), actual.Count());
            Assert.True(!expected.Except(actual).Any());
        }

        private static IEnumerable<Line> ParsePath(string path)
        {
            var curPos = new Point() { X = 0, Y = 0 };
            var steps = 0;
            foreach (var token in path.Split(','))
            {
                var direction = token[0];
                var length = int.Parse(token.AsSpan(1));
                var newPos = default(Point);

                newPos = direction switch
                {
                    'R' => new Point() { X = curPos.X + length, Y = curPos.Y },
                    'U' => new Point() { X = curPos.X, Y = curPos.Y + length },
                    'D' => new Point() { X = curPos.X, Y = curPos.Y - length },
                    'L' => new Point() { X = curPos.X - length, Y = curPos.Y },
                    _ => throw new Exception("Invalid direction!"),
                };
                yield return new Line() { P1 = curPos, P2 = newPos, StepsUntilHere = steps };

                steps += length;
                curPos = newPos;
            }
        }

        private static IEnumerable<Point> GetPathIntersections(IEnumerable<Line> path1, IEnumerable<Line> path2) =>
            path1
            .SelectMany(line1 => path2
                .SelectMany(line2 => line1.GetIntersections(line2)))
            .Where(x => x.X != 0 && x.X != 0);

        private static int Solution1(string path1, string path2)
        {
            var p1 = ParsePath(path1).ToArray();
            var p2 = ParsePath(path2).ToArray();
            return GetPathIntersections(p1, p2).Select(x => Math.Abs(x.X) + Math.Abs(x.Y)).Min();
        }

        private static int Solution2(string path1, string path2)
        {
            var p1 = ParsePath(path1).ToArray();
            var p2 = ParsePath(path2).ToArray();
            return GetPathIntersections2(p1, p2)
                .Select(item => 
                    item.line1.StepsUntilHere + 
                    item.line1.P1.Distance(item.intersection) +
                    item.line2.StepsUntilHere +
                    item.line2.P1.Distance(item.intersection))
                .Min();
        }

        private static IEnumerable<(Point intersection, Line line1, Line line2)> GetPathIntersections2(IEnumerable<Line> path1, IEnumerable<Line> path2) =>
            path1
            .SelectMany(line1 => path2
                .SelectMany(line2 => line1.GetIntersections(line2)
                    .Select(intersection => (intersection, line1, line2))))
            .Where(x => x.intersection.X != 0 && x.intersection.X != 0);

        private struct Point
        {
            public int X { get; init; }
            public int Y { get; init; }

            public int Distance(Point other) => (int)Math.Sqrt((X - other.X) * (X - other.X) + (Y - other.Y)*(Y - other.Y));
        }

        private struct Line
        {
            public Point P1 { get; init; }
            public Point P2 { get; init; }
            public int StepsUntilHere { get; init; }

            bool IsVertical => P1.X == P2.X;

            bool IsHorizontal => P1.Y == P2.Y;

            bool Contains(Point p)
            {
                if (IsHorizontal)
                {
                    if (p.Y != P1.Y)
                    {
                        return false;
                    }
                    return (P2.X >= P1.X && p.X >= P1.X && p.X <= P2.X) || (P2.X < P1.X && p.X >= P2.X && p.X <= P1.X);
                }
                else if (IsVertical)
                {
                    if (p.X != P1.X)
                    {
                        return false;
                    }
                    return (P2.Y >= P1.Y && p.Y >= P1.Y && p.Y <= P2.Y) || (P2.Y < P1.Y && p.Y >= P2.Y && p.Y <= P1.Y);
                }
                return false;
            }

            public IEnumerable<Point> GetIntersections(Line other)
            {
                if (IsVertical)
                {
                    if (other.IsHorizontal)
                    {
                        var p = new Point() { X = P1.X, Y = other.P1.Y };
                        if (other.Contains(p) && Contains(p))
                        {
                            yield return p;
                        }
                    }
                    else
                    {
                        if (!other.IsVertical)
                        {
                            throw new ArgumentException($"{nameof(other)} is not valid line in a grid!");
                        }

                        if (P1.X == other.P1.X)
                        {
                            var step = P2.Y >= P1.Y ? 1 : -1;
                            for (var i = P1.Y; (step == 1 && i <= P2.Y) || (step == -1 && i >= P2.Y); i += step)
                            {
                                var p = new Point() { X = other.P1.X, Y = i };
                                if (other.Contains(p))
                                {
                                    yield return p;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!IsHorizontal)
                    {
                        throw new ArgumentException($"this is not valid line in a grid!");
                    }

                    if (other.IsVertical)
                    {
                        var p = new Point() { X = other.P1.X, Y = P1.Y };
                        if (other.Contains(p) && Contains(p))
                        {
                            yield return p;
                        }
                    }
                    else
                    {
                        if (!other.IsHorizontal)
                        {
                            throw new ArgumentException($"{nameof(other)} is not valid line in a grid!");
                        }
                        if (P1.Y == other.P1.Y)
                        {
                            var step = P2.X >= P1.X ? 1 : -1;
                            for (var i = P1.X; (step == 1 && i <= P2.X) || (step == -1 && i >= P2.X); i += step)
                            {
                                var p = new Point() { X = i, Y = other.P1.Y };
                                if (other.Contains(p))
                                {
                                    yield return p;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
