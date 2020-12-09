using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2019
{
    public class Day08
    {
        [Fact]
        public void Test1()
        {
            var input = "121456789012";
            var layers = Enumerable
                .Range(0, input.Length)
                .Select(x => (x, input[x] - '0'))
                .GroupBy(g => g.x / (3 * 2)).Select(g => g.Select(t => t.Item2).ToArray()).ToArray();
            Assert.Equal(2, Solution1(layers));
        }

        [Fact]
        public void Test2()
        {
            var input = File.ReadAllText("input/day8.txt");
            var layers = Enumerable
                .Range(0, input.Length)
                .Select(x => (x, input[x] - '0'))
                .GroupBy(g => g.x / (25 * 6)).Select(g => g.Select(t => t.Item2).ToArray())
                .Where(a => a.Length == 25 * 6)
                .ToArray();
            Assert.Equal(1677, Solution1(layers));
        }

        [Fact]
        public void Test3()
        {
            var input = File.ReadAllText("input/day8.txt");

            var image = Solution2(
                Enumerable
                .Range(0, input.Length)
                .Select(x => (x, input[x] - '0'))
                .GroupBy(g => g.x / (25 * 6)).Select(g => g.Select(t => t.Item2).ToArray())
                .Where(a => a.Length == 25 * 6)
                .ToArray());

            var printedLines = new string[6];
            for (var i = 0; i < 6; i++)
            {
                printedLines[i] = string.Concat(image.Skip(25 * i).Take(25).Select(x => x != 0 ? 'X' : ' '));
            }

            Assert.Equal("X  X XXX  X  X XXXX XXX  ",printedLines[0]);
            Assert.Equal("X  X X  X X  X X    X  X ",printedLines[1]);
            Assert.Equal("X  X XXX  X  X XXX  X  X ",printedLines[2]);
            Assert.Equal("X  X X  X X  X X    XXX  ",printedLines[3]);
            Assert.Equal("X  X X  X X  X X    X    ",printedLines[4]);
            Assert.Equal(" XX  XXX   XX  X    X    ",printedLines[5]);

        }

        private static int Solution1(IEnumerable<int[]> layers)
        {
            var min = layers.Min(l => l.Count(c => c == 0));
            var minLayer = layers.First(l => l.Count(c => c == 0) == min);
            return minLayer.Count(c => c == 1) * minLayer.Count(c => c == 2);
        }

        private static int[] Solution2(IEnumerable<int[]> layers)
        {
            var ret = new int[layers.First().Length];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = layers.First(l => l[i] != 2)[i];
            }
            return ret;
        }
    }
}
