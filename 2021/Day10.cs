using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day10
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(26397, Part1(new[] {
                "[({(<(())[]>[[{[]{<()<>>",
                "[(()[<>])]({[<{<<[]>>(",
                "{([(<{}[<>[]}>{[]{[(<()>",
                "(((({<>}<{<{<>}{[]{[]{}",
                "[[<[([]))<([[{}[[()]]]",
                "[{[{({}]{}}([{[{{{}}([]",
                "{<[[]]>}<{[{[{[]{()[[[]",
                "[<(<(<(<{}))><([]([]()",
                "<{([([[(<>()){}]>(<<{{",
                "<{([{{}}[<[[[<>{}]]]>[]]"
            }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(311895, Part1(File.ReadAllLines("input/day10.txt")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(288957, Part2(new[] {
                "[({(<(())[]>[[{[]{<()<>>",
                "[(()[<>])]({[<{<<[]>>(",
                "{([(<{}[<>[]}>{[]{[(<()>",
                "(((({<>}<{<{<>}{[]{[]{}",
                "[[<[([]))<([[{}[[()]]]",
                "[{[{({}]{}}([{[{{{}}([]",
                "{<[[]]>}<{[{[{[]{()[[[]",
                "[<(<(<(<{}))><([]([]()",
                "<{([([[(<>()){}]>(<<{{",
                "<{([{{}}[<[[[<>{}]]]>[]]"
            }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(2904180541, Part2(File.ReadAllLines("input/day10.txt")));
        }

        private static int Part1(IEnumerable<string> input) => input
            .Select(ParseLine)
            .Select(x => x.firstIllegalChar)
            .Where(x => x != ' ')
            .Select(GetCharScore)
            .Sum();

        private static long Part2(IEnumerable<string> input)
        {
            var scores = input
                .Select(ParseLine)
                .Where(x => x.firstIllegalChar == ' ')
                .Select(x => (new[] { (long)0 })
                    .Concat(x.unmatchedChars
                                .Select(GetIncompleteCharScore))
                    .Aggregate((x, y) => x * 5 + y))
                .OrderBy(x => x).ToArray();
            return scores[scores.Length / 2];
        }

        private static (char firstIllegalChar, IEnumerable<char> unmatchedChars) ParseLine(string line)
        {
            var charStack = new Stack<char>();
            foreach (var curChar in line)
            {
                if (DoesCharOpenChunk(curChar))
                {
                    charStack.Push(curChar);
                    continue;
                }
                var openChar = charStack.Pop();
                if (!IsMatchingChuckChar(openChar, curChar))
                {
                    return (curChar, Array.Empty<char>());
                }
            }
            return (' ', charStack);
        }

        private static bool DoesCharOpenChunk(char x) =>
            x switch
            {
                '(' or '[' or '{' or '<' => true,
                _ => false,
            };
        private static bool IsMatchingChuckChar(char open, char close) =>
            open switch
            {
                '(' => close == ')',
                '[' => close == ']',
                '{' => close == '}',
                '<' => close == '>',
                _ => throw new ArgumentException(nameof(open)),
            };
        private static int GetCharScore(char x) =>
            x switch
            {
                ')' => 3,
                ']' => 57,
                '}' => 1197,
                '>' => 25137,
                _ => throw new ArgumentException(nameof(x)),
            };

        private static long GetIncompleteCharScore(char x) =>
            x switch
            {
                '(' => 1,
                '[' => 2,
                '{' => 3,
                '<' => 4,
                _ => throw new ArgumentException(nameof(x)),
            };

    }
}
