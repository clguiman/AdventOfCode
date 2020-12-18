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
                .Select(l => ExpressionEvaluator.Evaluate(l, ExpressionEvaluator.DefaultOperatorPrecendenceComparator))
                .Sum();

        private static long Part2(IEnumerable<string> input) => input
                .Select(l => ExpressionEvaluator.Evaluate(l, (a,b)=>
                    a == ExpressionEvaluator.Operator.Add && b == ExpressionEvaluator.Operator.Multiply ? 1 : 0))
                .Sum();

        private static class ExpressionEvaluator
        {
            public static long Evaluate(string expression, Func<Operator, Operator, int> operatorPrecendenceComparator)
            {
                Stack<Operator> operators = new();
                Stack<long> values = new();

                foreach (var (startIdx, endIdx) in Tokenize(expression))
                {
                    var token = expression.AsSpan(startIdx, endIdx - startIdx + 1);
                    if (token.Length == 1)
                    {
                        if (token[0] == '(')
                        {
                            operators.Push(Operator.OpenParanthesis);
                            continue;
                        }
                        if (token[0] == ')')
                        {
                            while (operators.Peek() != Operator.OpenParanthesis)
                            {
                                var a = values.Pop();
                                var b = values.Pop();
                                values.Push(ComputeOperation(a, b, operators.Pop()));
                            }
                            operators.Pop();
                            continue;
                        }
                        if (token[0] == '+' || token[0] == '*')
                        {
                            var op = token[0] == '+' ? Operator.Add : Operator.Multiply;
                            while (operators.Count > 0)
                            {
                                var topOp = operators.Peek();
                                if (topOp == Operator.OpenParanthesis ||
                                    topOp == Operator.ClosedParanthesis ||
                                    operatorPrecendenceComparator(op, topOp) > 0)
                                {
                                    break;
                                }
                                var a = values.Pop();
                                var b = values.Pop();
                                values.Push(ComputeOperation(a, b, operators.Pop()));
                            }
                            operators.Push(op);
                            continue;
                        }
                    }
                    if (long.TryParse(token, out long value))
                    {
                        values.Push(value);
                        continue;
                    }

                    throw new Exception($"Invalid token! {token.ToString()}");
                }

                while (operators.Count > 0)
                {
                    var a = values.Pop();
                    var b = values.Pop();
                    values.Push(ComputeOperation(a, b, operators.Pop()));
                }
                if (values.Count != 1)
                {
                    throw new Exception("values stack has more than 1 element!");
                }
                return values.Peek();
            }

            private static IEnumerable<(int startIdx, int endIdx)> Tokenize(string input)
            {
                var startIdx = 0;
                for (var idx = 0; idx < input.Length; idx++)
                {
                    if (char.IsWhiteSpace(input[idx]) ||
                        input[idx] == '(' ||
                        input[idx] == ')' ||
                        input[idx] == '+' ||
                        input[idx] == '*' ||
                        idx == input.Length - 1)
                    {
                        while (startIdx < idx && char.IsWhiteSpace(input[startIdx]))
                        {
                            startIdx++;
                        }
                        if (startIdx < idx)
                        {
                            yield return (startIdx, idx - 1);
                            startIdx = idx;
                        }
                        if (!char.IsWhiteSpace(input[idx]))
                        {
                            yield return (idx, idx);
                            startIdx++;
                        }
                    }
                }
            }

            private static long ComputeOperation(long a, long b, Operator op) => op switch
            {
                Operator.Add => a + b,
                Operator.Multiply => a * b,
                _ => throw new ArgumentException(nameof(op)),
            };

            public static int DefaultOperatorPrecendenceComparator(Operator a, Operator b) =>0;

            public enum Operator
            {
                OpenParanthesis,
                ClosedParanthesis,
                Add,
                Multiply
            }
        }
    }
}
