using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2022
{
    public class Day02
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(15, Part1(ParseInput(new[] { "A Y", "B X", "C Z" })));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(17189, Part1(ParseInput(File.ReadAllLines("input/day02.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(12, Part2(ParseInput2(new[] { "A Y", "B X", "C Z" })));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(13490, Part2(ParseInput2(File.ReadAllLines("input/day02.txt"))));
        }

        private static int Part1(IEnumerable<(HandShape opponent, HandShape response)> input) => 
            input.Select(x => (int)x.response + (x.response.Compare(x.opponent) + 1) * 3).Sum();

        private static int Part2(IEnumerable<(HandShape opponent, int outcome)> input) => 
            input.Select(x => (int)x.opponent.GetShapeToPlay(x.outcome) + (x.outcome + 1) * 3).Sum();

        private static IEnumerable<(HandShape opponent, HandShape response)> ParseInput(IEnumerable<string> input) => ParseLines(input)
                .Select(x => (opponent: (HandShape)(x.first + 1), response: (HandShape)(x.second + 1)));


        private static IEnumerable<(HandShape opponent, int outcome)> ParseInput2(IEnumerable<string> input) => ParseLines(input)
                .Select(x => (opponent: (HandShape)(x.first + 1), outcome: x.second - 1));

        private static IEnumerable<(int first, int second)> ParseLines(IEnumerable<string> input) => input
                .Select(x =>
                {
                    var t = x.Split(' ');
                    return (t[0][0], t[1][0]);
                })
                .Select(x => (x.Item1 - 'A', x.Item2 - 'X'));
    }

    internal enum HandShape
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3,
    }

    internal static class HandShapeExtension
    {
        public static int Compare(this HandShape lhs, HandShape rhs)
        {
            if (lhs == rhs)
            {
                return 0;
            }
            switch (lhs)
            {
                case HandShape.Paper: return rhs == HandShape.Rock ? 1 : -1;
                case HandShape.Rock: return rhs == HandShape.Paper ? -1 : 1;
                case HandShape.Scissors: return rhs == HandShape.Paper ? 1 : -1;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public static HandShape GetShapeToPlay(this HandShape lhs, int comparison)
        {
            if (comparison == 0)
            {
                return lhs;
            }
            switch (lhs)
            {
                case HandShape.Paper: return comparison == -1 ? HandShape.Rock : HandShape.Scissors;
                case HandShape.Rock: return comparison == -1 ? HandShape.Scissors : HandShape.Paper;
                case HandShape.Scissors: return comparison == -1 ? HandShape.Paper : HandShape.Rock;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
