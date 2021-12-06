using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day06
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(5934, SimulateLifetime(new[] { 3, 4, 3, 1, 2 }, 80));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(372984, SimulateLifetime(File.ReadAllText("input/day06.txt").Split(',').Select(int.Parse), 80));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(1681503251694, SimulateLifetime(File.ReadAllText("input/day06.txt").Split(',').Select(int.Parse), 256));
        }

        private static long SimulateLifetime(IEnumerable<int> input, int days)
        {
            var curState = new List<long>();
            curState.AddRange(new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            foreach (var item in input)
            {
                curState[item]++;
            }

            for (var day = 0; day < days; day++)
            {
                var zeros = curState[0];
                curState.RemoveAt(0);
                curState.AddRange(new[] { zeros });
                curState[6] += zeros;
            }
            return curState.Sum();
        }
    }
}
