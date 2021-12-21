using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2018
{
    public class Day05
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("aabAAB", React("aabAAB"));
            Assert.Equal("dabCBAcaDA", React("dabAcCaCBAcCcaDA"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(9116, React(File.ReadAllText("input/day05.txt")).Length);
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(4, Part2("dabAcCaCBAcCcaDA"));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(6890, Part2(File.ReadAllText("input/day05.txt")));
        }

        public static string React(string input)
        {
            Stack<char> stack = new();
            foreach (var c in input)
            {
                var top = stack.Count == 0 ? '\0' : stack.Peek();
                if (char.ToLowerInvariant(top) == char.ToLowerInvariant(c) && top != c)
                {
                    stack.Pop();
                }
                else
                {
                    stack.Push(c);
                }
            }
            return string.Concat(stack.Reverse());
        }

        public static int Part2(string input) =>
            React(input).Distinct().Select(char.ToLowerInvariant).Distinct()
                .Select(c => string.Concat(input.ToCharArray().Where(t => t != c && t != char.ToUpperInvariant(c))))
                .Select(x => React(x).Length)
                .Min();
    }
}
