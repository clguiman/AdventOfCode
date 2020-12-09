using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2019
{
    public class Day09
    {
        [Fact]
        public void Test1()
        {
            var emulator = new IntCodeEmulator(new long[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 }, true);
            List<long> output = new();
            emulator.Run(System.Array.Empty<long>(), output);
            AssertCollection(new long[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 }, output);
        }

        [Fact]
        public void Test2()
        {
            var emulator = new IntCodeEmulator(new long[] { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 }, true);
            List<long> output = new();
            emulator.Run(System.Array.Empty<long>(), output);
            Assert.Equal(1219070632396864, output[0]);
        }

        [Fact]
        public void Test3()
        {
            var emulator = new IntCodeEmulator(new long[] { 104, 1125899906842624, 99 }, true);
            List<long> output = new();
            emulator.Run(System.Array.Empty<long>(), output);
            Assert.Equal(1125899906842624, output[0]);
        }

        [Fact]
        public void Test4()
        {
            var emulator = new IntCodeEmulator(File.ReadAllText("input/day09.txt").Split(',').Select(long.Parse).ToArray(), true);
            List<long> output = new();
            emulator.Run(new long[] { 1 }, output);
            Assert.Single(output);
            Assert.Equal(2752191671, output[0]);
        }

        [Fact]
        public void Test5()
        {
            var emulator = new IntCodeEmulator(File.ReadAllText("input/day09.txt").Split(',').Select(long.Parse).ToArray(), true);
            List<long> output = new();
            emulator.Run(new long[] { 2 }, output);
            Assert.Single(output);
            Assert.Equal(87571, output[0]);
        }

        private void AssertCollection<Type>(IEnumerable<Type> expected, IEnumerable<Type> actual)
        {
            Assert.Equal(expected.Count(), actual.Count());
            Assert.True(!expected.Except(actual).Any());
        }
    }
}
