using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2022
{
    public class Day01
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(67633, ParseInput().Max());
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(199628, ParseInput().OrderByDescending(x => x).Take(3).Sum());
        }

        private static IEnumerable<int> ParseInput()
        {
            int sum = 0;
            foreach (var line in File.ReadAllLines("input/day01.txt"))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    yield return sum;
                    sum = 0;
                }
                else
                {
                    sum += int.Parse(line);
                }
            }
            yield return sum;
        }
    }
}
