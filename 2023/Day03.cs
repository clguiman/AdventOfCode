using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2023
{
    public class Day03
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(4361, SolvePart1(ParseInput([
                "467..114..",
                "...*......",
                "..35..633.",
                "......#...",
                "617*......",
                ".....+.58.",
                "..592.....",
                "......755.",
                "...$.*....",
                ".664.598.."
                ])));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(540025, SolvePart1(ParseInput(File.ReadAllLines("input/day03.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(467835, SolvePart2(ParseInput([
                "467..114..",
                "...*......",
                "..35..633.",
                "......#...",
                "617*......",
                ".....+.58.",
                "..592.....",
                "......755.",
                "...$.*....",
                ".664.598.."
                ])));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(84584891, SolvePart2(ParseInput(File.ReadAllLines("input/day03.txt"))));
        }

        private static int SolvePart1(Grid2D<char> engine)
        {
            var numbers = GetNumbers(engine);

            return numbers.Where(n => Enumerable.Range(n.x1, n.x2 - n.x1 + 1)
                                         .Select(x => (x, y: n.row))
                                         .Any(t => engine.GetAllAdjacentLocations(t.x, t.y)
                                                    .Any(p => engine.At(p.x, p.y) != '.' && !char.IsNumber(engine.At(p.x, p.y)))))
                .Sum(t => t.value);
        }

        private static int SolvePart2(Grid2D<char> engine)
        {
            var numbers = GetNumbers(engine);
            return engine.Enumerate()
                .Where(t => t.value == '*')
                .Select(t => (t.x, t.y))
                .Select(gear =>
                {
                    var ratios = numbers.Where(n => engine.GetAllAdjacentLocations(gear.x, gear.y).Any(t => t.y == n.row && t.x >= n.x1 && t.x <= n.x2)).ToArray();
                    if (ratios.Length == 2)
                    {
                        return ratios[0].value * ratios[1].value;
                    }
                    return 0;
                })
                .Sum();
        }

        private static List<(int row, int x1, int x2, int value)> GetNumbers(Grid2D<char> engine)
        {
            var digits = engine.Enumerate().Where(t => char.IsNumber(t.value)).ToArray();
            var curNumber = (row: digits[0].y, x1: digits[0].x, x2: digits[0].x, value: digits[0].value - '0');
            List<(int row, int x1, int x2, int value)> numbers = new();
            foreach (var digit in digits.Skip(1))
            {
                if (digit.y == curNumber.row && curNumber.x2 == digit.x - 1)
                {
                    curNumber.x2 = digit.x;
                    curNumber.value = curNumber.value * 10 + (digit.value - '0');
                }
                else
                {
                    numbers.Add(curNumber);
                    curNumber = (row: digit.y, x1: digit.x, x2: digit.x, value: digit.value - '0');
                }
            }
            numbers.Add(curNumber);
            return numbers;
        }

        private static Grid2D<char> ParseInput(IEnumerable<string> input) => new(input.Select(c => c));
    }
}
