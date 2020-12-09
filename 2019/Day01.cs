using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2019
{
    public class Day01
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(2, Solution(new[] { 12 }));
            Assert.Equal(33583, Solution(new[] { 100756 }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(3390596, Solution(File.ReadAllLines("input/day1.txt").Select(int.Parse)));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(2, Solution2(new[] { 14 }));
            Assert.Equal(966, Solution2(new[] { 1969 }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(5083024, Solution2(File.ReadAllLines("input/day1.txt").Select(int.Parse)));
        }

        private int Solution(IEnumerable<int> input) => input.Select(x => x / 3 - 2).Sum();

        private int Solution2(IEnumerable<int> input) => input
            .Select(x =>
            {
                var sum = x / 3 - 2;
                var fuel = sum;
                while (fuel > 0)
                {
                    fuel = fuel / 3 - 2;
                    if (fuel > 0)
                    {
                        sum += fuel;
                    }
                }
                return sum;
            })
            .Sum();
    }
}
