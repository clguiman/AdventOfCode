using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2018
{
    public class Day01
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(590, File.ReadAllLines("input/day01.txt").Select(int.Parse).Sum());
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(14, Part2(new[] { 7, 7, -2, -7, -4 }));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(83445, Part2(File.ReadAllLines("input/day01.txt").Select(int.Parse).ToArray()));
        }

        public static int Part2(int[] input)
        {
            HashSet<int> frequencies = new();
            var sum = 0;
            for (var idx = 0; idx < input.Length; idx++)
            {
                sum += input[idx];
                if (idx == input.Length - 1)
                {
                    idx = -1;
                }
                if (!frequencies.Contains(sum))
                {
                    frequencies.Add(sum);
                    continue;
                }
                return sum;
            }
            return 0;
        }
    }
}
