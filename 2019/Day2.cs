using System.IO;
using System.Linq;
using Xunit;

namespace _2019
{
    public class Day2
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(2, SolutionBeforePatch(new int[] { 1, 0, 0, 0, 99 }));
            Assert.Equal(2, SolutionBeforePatch(new int[] { 2, 3, 0, 3, 99 }));
            Assert.Equal(3500, SolutionBeforePatch(new int[] { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 }));
            Assert.Equal(30, SolutionBeforePatch(new int[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 }));
        }

        [Fact]
        public void Test2()
        {
            var input = File.ReadAllText("input/day2.txt").Split(',').Select(int.Parse).ToArray();
            input[1] = 12;
            input[2] = 2;
            Assert.Equal(3790689, SolutionBeforePatch(input));
        }

        [Fact]
        public void Test3()
        {
            var input = File.ReadAllText("input/day2.txt").Split(',').Select(int.Parse).ToArray();

            Assert.Equal(6533, Solution2(input));
        }

        private int SolutionBeforePatch(int[] input)
        {
            IntCodeEmulator emulator = new(input);
            emulator.Run();
            return emulator.ReadMemory(0);
        }

        private int Solution2(int[] input)
        {
            for (var i = 0; i <= 99; i++)
            {
                for (var j = 0; j <= 99; j++)
                {
                    IntCodeEmulator emulator = new(input);
                    emulator.WriteMemory(1, i);
                    emulator.WriteMemory(2, j);
                    emulator.Run();
                    input[1] = i;
                    input[2] = j;
                    if (19690720 == emulator.ReadMemory(0))
                    {
                        return 100 * i + j;
                    }
                }
            }
            return 0;
        }
    }
}
