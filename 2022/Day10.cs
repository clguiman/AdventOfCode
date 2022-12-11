using System;
using System.Collections.Generic;
using System.IO;
using Utils;
using Xunit;

namespace _2022
{
    public class Day10
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(14820, Part1(File.ReadAllLines("input/day10.txt")));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(
                "###..####.####.#..#.####.####.#..#..##..\r\n" +
                "#..#....#.#....#.#..#....#....#..#.#..#.\r\n" +
                "#..#...#..###..##...###..###..####.#..#.\r\n" +
                "###...#...#....#.#..#....#....#..#.####.\r\n" +
                "#.#..#....#....#.#..#....#....#..#.#..#.\r\n" +
                "#..#.####.####.#..#.####.#....#..#.#..#.\r\n",
                Part2(File.ReadAllLines("input/day10.txt")));
        }

        private static int Part1(IEnumerable<string> input)
        {
            var nextSignalCycle = 20;
            var ret = 0;
            var cycles = 0;
            EmulateProgram(input, (int x) =>
            {
                cycles++;
                if (nextSignalCycle <= cycles)
                {
                    ret += nextSignalCycle * x;
                    nextSignalCycle += 40;
                }
            });
            return ret;
        }

        private static string Part2(IEnumerable<string> input)
        {
            Grid2D<char> crt = new(40, 6);
            var crtPos = 0;
            EmulateProgram(input, (int x) =>
            {
                var crtXPos = crtPos % 40;
                var crtYPos = crtPos / 40;
                if (x == crtXPos || x + 1 == crtXPos || x - 1 == crtXPos)
                {
                    crt.AtRef(crtXPos, crtYPos) = '#';
                }
                else
                {
                    crt.AtRef(crtXPos, crtYPos) = '.';
                }
                crtPos++;
            });
            return crt.ToString(null);
        }

        private static void EmulateProgram(IEnumerable<string> input, Action<int> observerAction)
        {
            var x = 1;
            foreach (var xAddVal in ParseInput(input))
            {
                observerAction(x);
                x += xAddVal;
            }
        }

        private static IEnumerable<int> ParseInput(IEnumerable<string> input)
        {
            foreach (var line in input)
            {
                var tokens = line.Split(' ');
                if (tokens.Length == 1)
                {
                    // no op
                    yield return 0;
                }
                else
                {
                    yield return 0;
                    yield return int.Parse(tokens[1]);
                }
            }
        }
    }
}