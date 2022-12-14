using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2022
{
    public class Day14
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(24, Solve(ParseInput(new[] {
                "498,4 -> 498,6 -> 496,6",
                "503,4 -> 502,4 -> 502,9 -> 494,9"
            }), stopWhenFull: false));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(979, Solve(ParseInput(File.ReadAllLines("input/day14.txt")), stopWhenFull: false));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(93, Solve(ParseInput(new[] {
                "498,4 -> 498,6 -> 496,6",
                "503,4 -> 502,4 -> 502,9 -> 494,9"
            }), stopWhenFull: true));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(29044, Solve(ParseInput(File.ReadAllLines("input/day14.txt")), stopWhenFull: true));
        }

        private static int Solve(IEnumerable<Line> lines, bool stopWhenFull)
        {
            var abyssLine = lines.Where(l => l.IsHorizontal).Select(l => l.startY).Max();
            Grid2D<int> map = new(700, abyssLine + 3);
            foreach (var line in lines.Concat(new[] { new Line(0, abyssLine + 2, map.Width - 1, abyssLine + 2) }))
            {
                if (line.IsHorizontal)
                {
                    for (var idx = line.startX; idx <= line.endX; idx++)
                    {
                        map.SetAt(1, idx, line.startY);
                    }
                }
                else
                {
                    for (var idx = line.startY; idx <= line.endY; idx++)
                    {
                        map.SetAt(1, line.startX, idx);
                    }
                }
            }

            for (var sandGrains = 0; true; sandGrains++)
            {
                var sandX = 500;
                var sandY = 0;
                while (true)
                {
                    if (!stopWhenFull && sandY > abyssLine)
                    {
                        return sandGrains;
                    }
                    if (map.At(sandX, sandY + 1) == 0)
                    {
                        sandY++;
                        continue;
                    }
                    if (map.At(sandX - 1, sandY + 1) == 0)
                    {
                        sandX--;
                        sandY++;
                        continue;
                    }
                    if (map.At(sandX + 1, sandY + 1) == 0)
                    {
                        sandX++;
                        sandY++;
                        continue;
                    }
                    map.SetAt(2, sandX, sandY);
                    break;
                }
                if (stopWhenFull && sandX == 500 && sandY == 0)
                {
                    return sandGrains + 1;
                }
            }
        }

        private static IEnumerable<Line> ParseInput(IEnumerable<string> input)
        {
            var parsedLines = input
                .Select(x => x.Split('>')
                              .SelectMany(y => y.Split('-'))
                              .Select(y => y.Trim())
                              .Where(x => !string.IsNullOrEmpty(x))
                              .Select(x =>
                              {
                                  var t = x.Split(',').Select(int.Parse).ToArray();
                                  return (x: t[0], y: t[1]);
                              })
                              .ToArray()
                              );
            foreach (var items in parsedLines)
            {
                for (var idx = 1; idx < items.Length; idx++)
                {
                    yield return new Line(items[idx - 1].x, items[idx - 1].y, items[idx].x, items[idx].y);
                }
            }
        }

        private record Line
        {
            public Line(int startX, int startY, int endX, int endY)
            {
                if (startY == endY)
                {
                    this.startX = Math.Min(startX, endX);
                    this.endX = Math.Max(startX, endX);
                    this.startY = startY;
                    this.endY = startY;
                }
                else
                {
                    this.startY = Math.Min(startY, endY);
                    this.endY = Math.Max(startY, endY);
                    this.startX = startX;
                    this.endX = startX;
                }
            }

            public readonly int startX;
            public readonly int startY;
            public readonly int endX;
            public readonly int endY;

            public bool IsHorizontal => startY == endY;
        }
    }
}