using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2022
{
    public class Day13
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(13, Part1(ParseInput(new[] {
                    "[1,1,3,1,1]",
                    "[1,1,5,1,1]",
                    "",
                    "[[1],[2,3,4]]",
                    "[[1],4]",
                    "",
                    "[9]",
                    "[[8,7,6]]",
                    "",
                    "[[4,4],4,4]",
                    "[[4,4],4,4,4]",
                    "",
                    "[7,7,7,7]",
                    "[7,7,7]",
                    "",
                    "[]",
                    "[3]",
                    "",
                    "[[[]]]",
                    "[[]]",
                    "",
                    "[1,[2,[3,[4,[5,6,7]]]],8,9]",
                    "[1,[2,[3,[4,[5,6,0]]]],8,9]"
            })));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(6395, Part1(ParseInput(File.ReadAllLines("input/day13.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(140, Part2(ParseInput(new[] {
                    "[1,1,3,1,1]",
                    "[1,1,5,1,1]",
                    "",
                    "[[1],[2,3,4]]",
                    "[[1],4]",
                    "",
                    "[9]",
                    "[[8,7,6]]",
                    "",
                    "[[4,4],4,4]",
                    "[[4,4],4,4,4]",
                    "",
                    "[7,7,7,7]",
                    "[7,7,7]",
                    "",
                    "[]",
                    "[3]",
                    "",
                    "[[[]]]",
                    "[[]]",
                    "",
                    "[1,[2,[3,[4,[5,6,7]]]],8,9]",
                    "[1,[2,[3,[4,[5,6,0]]]],8,9]"
            })));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(24921, Part2(ParseInput(File.ReadAllLines("input/day13.txt"))));
        }

        private static int Part1(IEnumerable<TreeNode> packets) => packets
            .Select((packet, idx) => (packet, pairId: idx / 2))
            .GroupBy(t => t.pairId)
            .Select(t => (
                first: t.Select(x => x.packet).First(),
                second: t.Select(x => x.packet).Last())
            )
            .Select((pair, idx) => (pair, idx: idx + 1))
            .Where(item => -1 == item.pair.first.Compare(item.pair.second))
            .Select(item => item.idx)
            .Sum();

        private static int Part2(IEnumerable<TreeNode> packets)
        {
            var dividerPackets = ParseInput(new[] { "[[2]]", "[[6]]" }).ToArray();
            return  (packets.Count(x => 1 != x.Compare(dividerPackets[0])) + 1) * 
                    (packets.Count(x => 1 != x.Compare(dividerPackets[1])) + 2);
        }

        private static IEnumerable<TreeNode> ParseInput(IEnumerable<string> input) => input
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => ParseRawPackets(x));

        private static TreeNode ParseRawPackets(ReadOnlySpan<char> rawPackets)
        {
            var parsingStack = new Stack<List<TreeNode>>();
            var input = rawPackets.Trim();
            while (input.Length > 0)
            {
                if (char.IsWhiteSpace(input[0]) || input[0] == ',')
                {
                    input = input[1..];
                    continue;
                }
                if (input[0] == '[')
                {
                    parsingStack.Push(new());
                    input = input[1..];
                    continue;
                }
                if (input[0] == ']')
                {
                    var newNode = new TreeNode() { Number = null, Children = parsingStack.Pop() };
                    if (parsingStack.Count == 0)
                    {
                        parsingStack.Push(new(newNode.Children));
                    }
                    else
                    {
                        parsingStack.Peek().Add(newNode);
                    }
                    input = input[1..];
                    continue;
                }
                var numberEndPos = 0;
                while (input.Length > numberEndPos - 1 && input[numberEndPos + 1] >= '0' && input[numberEndPos + 1] <= '9')
                {
                    numberEndPos++;
                }
                parsingStack.Peek().Add(new TreeNode() { Number = int.Parse(input[0..(numberEndPos + 1)]), Children = new() });
                input = input[(numberEndPos + 1)..];
            }
            if (parsingStack.Count != 1)
            {
                throw new Exception("The parsing stack should have only one item!");
            }

            return new() { Number = null, Children = parsingStack.Pop() };
        }

        private class TreeNode
        {
            public int? Number { get; init; }
            public List<TreeNode> Children { get; init; }

            public int Compare(TreeNode rhs)
            {
                if (Number.HasValue && rhs.Number.HasValue)
                {
                    if (Number.Value == rhs.Number.Value)
                    {
                        return 0;
                    }
                    return (Number.Value < rhs.Number.Value) ? -1 : 1;
                }
                if (!Number.HasValue && rhs.Number.HasValue)
                {
                    TreeNode fakeNode = new() { Number = null, Children = new() { rhs } };
                    return Compare(fakeNode);
                }
                if (Number.HasValue && !rhs.Number.HasValue)
                {
                    return -1 * rhs.Compare(this);
                }

                for (var idx = 0; idx < Children.Count; idx++)
                {
                    if (rhs.Children.Count <= idx)
                    {
                        return 1;
                    }
                    var compareResult = Children[idx].Compare(rhs.Children[idx]);
                    if (compareResult != 0)
                    {
                        return compareResult;
                    }
                }

                if (Children.Count == rhs.Children.Count)
                {
                    return 0;
                }

                return -1;
            }
        }
    }
}