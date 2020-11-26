using Microsoft.VisualStudio.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace _2019
{
    public class Day7
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(43210, RunWithInput(
                new[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 },
                new[] { 4, 3, 2, 1, 0 }));
            Assert.Equal(54321, RunWithInput(
                new[] { 3, 23, 3, 24, 1002, 24, 10, 24, 1002, 23, -1, 23, 101, 5, 23, 23, 1, 24, 23, 23, 4, 23, 99, 0, 0 },
                new[] { 0, 1, 2, 3, 4 }));
            Assert.Equal(65210, RunWithInput(
                new[] { 3, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33, 1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0 },
                new[] { 1, 0, 4, 3, 2 }));
        }

        [Fact]
        public void Test2()
        {
            var code = File.ReadAllText("input/day7.txt").Split(',').Select(int.Parse).ToArray();
            Assert.Equal(422858,
                GenerateUniqueInput(0, 5)
                .Select(input => RunWithInput(code, input))
                .Max());
        }

        [Fact]
        public async Task Test3Async()
        {
            Assert.Equal(139629729, await RunInFeedbackLoopWithInputAsync(
                new[] { 3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26, 27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5 },
                new[] { 9, 8, 7, 6, 5 }));

            Assert.Equal(18216, await RunInFeedbackLoopWithInputAsync(
                new[] { 3, 52, 1001, 52, -5, 52, 3, 53, 1, 52, 56, 54, 1007, 54, 5, 55, 1005, 55, 26, 1001, 54, -5, 54,
                    1105, 1, 12, 1, 53, 54, 53, 1008, 54, 0, 55, 1001, 55, 1, 55, 2, 53, 55, 53, 4, 53, 1001, 56, -1,
                    56, 1005, 56, 6, 99, 0, 0, 0, 0, 10 },
                new[] { 9, 7, 8, 5, 6 }));
        }

        [Fact]
        public void Test4()
        {
            var code = File.ReadAllText("input/day7.txt").Split(',').Select(int.Parse).ToArray();
            Assert.Equal(14897241,
                GenerateUniqueInput(5, 5)
                .Select(input => RunInFeedbackLoopWithInput(code, input))
                .Max());
        }

        private static int RunWithInput(int[] code, int[] input)
        {
            var amplifierInput = 0;
            for (var i = 0; i < input.Length; i++)
            {
                List<int> output = new();
                var emulator = new IntCodeEmulator(code);
                emulator.Run(new[] { input[i], amplifierInput }, output);
                amplifierInput = output[0];
            }
            return amplifierInput;
        }

        private static int RunInFeedbackLoopWithInput(int[] code, int[] input)
        {
            return new JoinableTaskFactory(new JoinableTaskContext()).Run(async () => await RunInFeedbackLoopWithInputAsync(code, input));
        }
        private static async Task<int> RunInFeedbackLoopWithInputAsync(int[] code, int[] input)
        {
            List<int> previousOutput = new() { 0 };
            var amplifiers = Enumerable.Range(0, input.Length).Select(_ => new IntCodeEmulator(code)).ToArray();
            var amplifiersInputs = input.Select(x => new AsyncQueue<int>()).ToArray();
            for (var i = 0; i < amplifiers.Length; i++)
            {
                amplifiersInputs[i].Enqueue(input[i]);
            }
            amplifiersInputs[0].Enqueue(0);

            await Task.WhenAll(
                Enumerable.Range(0, input.Length)
                .Select(x => amplifiers[x].RunAsync(new IntCodeEmulator.AsyncQueueIO(amplifiersInputs[x], x == input.Length - 1 ? amplifiersInputs[0] : amplifiersInputs[x + 1]), default))
                .ToArray());

            return await amplifiersInputs[0].DequeueAsync();
        }

        private static IEnumerable<int[]> GenerateUniqueInput(int rangeMin, int rangeCount)
        {
            var uniqueSample = Enumerable.Range(rangeMin, rangeCount).ToArray();
            return Enumerable.Range(rangeMin, rangeCount)
                .SelectMany(first => Enumerable.Range(rangeMin, rangeCount)
                    .SelectMany(second => Enumerable.Range(rangeMin, rangeCount)
                        .SelectMany(third => Enumerable.Range(rangeMin, rangeCount)
                            .SelectMany(forth => Enumerable.Range(rangeMin, rangeCount)
                                .Select(fifth => new[] { first, second, third, forth, fifth })))))
                .Where(input => !uniqueSample.Except(input.Intersect(uniqueSample)).Any());
        }
    }
}
