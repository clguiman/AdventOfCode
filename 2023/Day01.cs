using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2023
{
    public class Day01
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(142, SolvePart1([
                "1abc2",
                "pqr3stu8vwx",
                "a1b2c3d4e5f",
                "treb7uchet"
                ]));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(55386, SolvePart1(File.ReadAllLines("input/day01.txt")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(281, SolvePart2([
                "two1nine",
                "eightwothree",
                "abcone2threexyz",
                "xtwone3four",
                "4nineeightseven2",
                "zoneight234",
                "7pqrstsixteen"
                ]));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(54824, SolvePart2(File.ReadAllLines("input/day01.txt")));
        }

        private static int SolvePart1(IEnumerable<string> input) => input
                .Select(line => line.Where(char.IsNumber).Select(c => (int)(c - '0')).ToArray())
                .Select(l => l.First() * 10 + l.Last())
                .Sum();

        private static int SolvePart2(IEnumerable<string> input) => SolvePart1(input
                .Select(l => l.Replace("one", "o1e"))
                .Select(l => l.Replace("two", "t2o"))
                .Select(l => l.Replace("three", "t3e"))
                .Select(l => l.Replace("four", "f4r"))
                .Select(l => l.Replace("five", "f5e"))
                .Select(l => l.Replace("six", "s6x"))
                .Select(l => l.Replace("seven", "s7n"))
                .Select(l => l.Replace("eight", "e8t"))
                .Select(l => l.Replace("nine", "n9e")));
    }
}
