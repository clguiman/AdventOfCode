using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace _2020
{
    public class Day18
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(54, Part1(new[]{
                "1 + 2",
                "1 + (2 * 3) + (4 * (5 + 6))"
            }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(5374004645253, Part1(File.ReadAllLines("input/day18.txt")));
        }

        [Theory]
        [InlineData("1 + 2 * 3 + 4 * 5 + 6", 231L)]
        [InlineData("1 + (2 * 3) + (4 * (5 + 6))", 51L)]
        [InlineData("2 * 3 + (4 * 5)", 46L)]
        [InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
        [InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
        [InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
        public void Test3(string e, long r)
        {
            Assert.Equal(r, Part2(new[] { e }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(88782789402798, Part2(File.ReadAllLines("input/day18.txt")));
        }

        private static long Part1(IEnumerable<string> input) => input
                .Select(l => Expression.Parse(l.AsSpan()))
                .Select(e => e.Resolve())
                .Sum();

        private static long Part2(IEnumerable<string> input) => input
                //.Select(ChangePrecendence)
                .Select(l => Expression.Parse(l.AsSpan()))
                .Select(e => e.Resolve())
                .Sum();

        private class Expression
        {
            public static Expression Parse(ReadOnlySpan<char> input)
            {
                var ret = new Expression();
                long curNumber = 0;
                Operation curOperation = Operation.Add;
                for (var idx = 0; idx < input.Length; idx++)
                {
                    var c = input[idx];
                    if (char.IsWhiteSpace(c))
                    {
                        continue;
                    }

                    if (char.IsNumber(c))
                    {
                        curNumber = curNumber * 10 + (c - '0');
                        continue;
                    }
                    else if (!char.IsNumber(c))
                    {
                        if (curNumber != 0)
                        {
                            ret.dependents.Add((curOperation, new Expression() { resolvedValue = curNumber, isExpressionResolved = true }));
                        }
                        curNumber = 0;
                    }
                    if (c == AdditionToken || c == MultiplicationToken)
                    {
                        curOperation = c == AdditionToken ? Operation.Add : Operation.Multiply;
                        continue;
                    }
                    if (c == OpenParanthesisToken)
                    {
                        var closedParanthesisPos = GetCorespondingClosedParanthesisPos(input.Slice(idx + 1));
                        ret.dependents.Add((curOperation, Parse(input.Slice(idx + 1, closedParanthesisPos))));
                        idx = idx + closedParanthesisPos + 1;
                        continue;
                    }
                }
                if (curNumber != 0)
                {
                    ret.dependents.Add((curOperation, new Expression() { resolvedValue = curNumber, isExpressionResolved = true }));
                }
                return ret;
            }

            private static int GetCorespondingClosedParanthesisPos(ReadOnlySpan<char> input)
            {
                var c = 1;
                for (var idx = 0; idx < input.Length; idx++)
                {
                    if (input[idx] == ')')
                    {
                        c--;
                    }
                    else if (input[idx] == '(')
                    {
                        c++;
                    }
                    if (c == 0)
                    {
                        return idx;
                    }
                }
                return -1;
            }

            public long Resolve()
            {
                if (isExpressionResolved)
                {
                    return resolvedValue;
                }
                long result = 0L;
                foreach (var cur in dependents)
                {
                    var curValue = cur.expression.Resolve();
                    if (cur.precedingOperation == Operation.Add)
                    {
                        result += curValue;
                    }
                    else
                    {
                        result *= curValue;
                    }
                }

                return result;
            }

            private long resolvedValue = -1L;
            private bool isExpressionResolved = false;
            List<(Operation precedingOperation, Expression expression)> dependents = new();


            public override string ToString()
            {
                if (isExpressionResolved)
                {
                    return resolvedValue.ToString();
                }

                var sb = new StringBuilder();
                foreach (var d in dependents)
                {
                    sb.Append('[');
                    sb.Append(d.precedingOperation == Operation.Add ? '+' : '*');
                    sb.Append(d.expression.ToString());
                    sb.Append(']');
                }
                return sb.ToString();
            }
            private enum Operation
            {
                Add,
                Multiply
            }

            private static readonly char AdditionToken = '+';
            private static readonly char MultiplicationToken = '*';
            private static readonly char WhitespaceToken = ' ';
            private static readonly char OpenParanthesisToken = '(';
            private static readonly char ClosedParanthesisToken = ')';
        }
    }
}
