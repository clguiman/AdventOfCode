using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2019
{
    public class Day5
    {
        [Fact]
        public void Test1()
        {
            var emulator = new IntCodeEmulator(new int[] { 1002, 4, 3, 4, 33 });
            emulator.Run();
            Assert.Equal(99, emulator.ReadMemory(4));
        }

        [Fact]
        public void Test2()
        {
            var emulator = new IntCodeEmulator(File.ReadAllText("input/day5.txt").Split(',').Select(int.Parse).ToArray());
            var output = new List<int>();
            emulator.Run(new[] { 1 }, output);

            for (var i = 0; i < output.Count - 1; i++)
            {
                Assert.Equal(0, output[i]);
            }

            Assert.Equal(7157989, output.Last());
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(1, RunWithSingleInput(new int[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 }, 8));
            Assert.Equal(0, RunWithSingleInput(new int[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 }, 9));
            Assert.Equal(1, RunWithSingleInput(new int[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 }, 8));
            Assert.Equal(0, RunWithSingleInput(new int[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 }, 9));
            Assert.Equal(0, RunWithSingleInput(new int[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 }, 9));
            Assert.Equal(1, RunWithSingleInput(new int[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 }, 7));
            Assert.Equal(0, RunWithSingleInput(new int[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 }, 9));
            Assert.Equal(1, RunWithSingleInput(new int[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 }, 7));

            Assert.Equal(0, RunWithSingleInput(new int[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 }, 0));
            Assert.Equal(1, RunWithSingleInput(new int[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 }, 1));
            Assert.Equal(0, RunWithSingleInput(new int[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 }, 0));
            Assert.Equal(1, RunWithSingleInput(new int[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 }, 1));

            Assert.Equal(999, RunWithSingleInput(new int[] {
                3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
                1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
                999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 5));

            Assert.Equal(1000, RunWithSingleInput(new int[] {
                3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
                1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
                999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 8));

            Assert.Equal(1001, RunWithSingleInput(new int[] {
                3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
                1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
                999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 42));

            Assert.Equal(7873292, RunWithSingleInput(File.ReadAllText("input/day5.txt").Split(',').Select(int.Parse).ToArray(), 5));
        }

        private static int RunWithSingleInput(int[] code, int inputValue)
        {
            var emulator = new IntCodeEmulator(code);
            var output = new List<int>();

            emulator.Run(new[] { inputValue }, output);

            Assert.Single(output);
            return output[0];
        }
    }
}
