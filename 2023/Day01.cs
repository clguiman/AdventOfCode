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
            Assert.Equal(1, File.ReadAllLines("input/day01.txt").Select(int.Parse).Sum());
        }
    }
}
