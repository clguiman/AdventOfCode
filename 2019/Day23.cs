using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace _2019
{
    public class Day23
    {
        [Fact]
        public async Task Part1TestAsync()
        {
            Assert.Equal(16685, (await SimulateNetworkAsync(File.ReadAllText("input/day23.txt").Split(',').Select(long.Parse).ToArray())).First().y);
        }

        [Fact]
        public async Task Part2TestAsync()
        {
            Assert.Equal(11048, (await SimulateNetworkAsync(File.ReadAllText("input/day23.txt").Split(',').Select(long.Parse).ToArray()))
                .GroupBy(t => t.y).Where(t => t.Count() >= 2).First().Key);
        }

        private static async Task<IEnumerable<(long x, long y)>> SimulateNetworkAsync(long[] input, int count = 50)
        {
            var computers = Enumerable.Range(0, count).Select(_ => new IntCodeEmulator(input, useLargeMemoryMode: true)).ToArray();
            var packetQueues = Enumerable.Range(0, count).Select(_ => new Queue<(long x, long y)>()).ToArray();
            var stepGuards = Enumerable.Range(0, count).Select(_ => new AsyncManualResetEvent(true)).ToArray();
            var stepCompletedSemaphore = new SemaphoreSlim(0, count);

            CancellationTokenSource cts = new();
            List<(long x, long y)> natOutput = new();
            List<(long x, long y)> natInput = new();

            int idleCount = 0;
            bool hasNatInput = false;

            async Task<T> StepGuardAsync<T>(int idx, Func<T> func, T defaultVal, CancellationToken cancellationToken)
            {
                try
                {
                    await stepGuards[idx].WaitAsync(cancellationToken);
                    return func();
                }
                catch (OperationCanceledException)
                {
                    return defaultVal;
                }
                finally
                {
                    stepGuards[idx].Reset();
                    stepCompletedSemaphore.Release();
                }
            }

            var tasks = Enumerable.Range(0, count)
                .Select(idx => (id: idx, nic: computers[idx], packetQueues: packetQueues[idx], inputQueue: new Queue<long>(new[] { (long)idx }), outputQueue: new Queue<long>()))
                .Select((t) => t.nic.RunAsync(new IntCodeEmulator.AsyncIO(
                      () => StepGuardAsync(t.id, () =>
                        {
                            if (t.inputQueue.Count == 0)
                            {
                                while (packetQueues[t.id].Count > 0)
                                {
                                    var packet = packetQueues[t.id].Dequeue();
                                    t.inputQueue.Enqueue(packet.x);
                                    t.inputQueue.Enqueue(packet.y);
                                }
                            }
                            var ret = t.inputQueue.Count == 0 ? -1 : t.inputQueue.Dequeue();
                            if (ret == -1)
                            {
                                Interlocked.Increment(ref idleCount);
                            }
                            return ret;
                        }, -1, cts.Token),
                     (value) => StepGuardAsync(t.id, () =>
                     {
                         t.outputQueue.Enqueue(value);
                         if (t.outputQueue.Count == 3)
                         {
                             var dest = t.outputQueue.Dequeue();
                             var x = t.outputQueue.Dequeue();
                             var y = t.outputQueue.Dequeue();

                             if (dest == 255)
                             {
                                 hasNatInput = true;
                                 natInput.Add((x, y));
                             }
                             else
                             {
                                 packetQueues[dest].Enqueue((x, y));
                             }
                         }
                         return true;
                     }, true, cts.Token)),
                cts.Token))
                .ToArray();

            async Task NatControllerAsync()
            {
                var consecutiveIdleSteps = 0;
                while (!cts.IsCancellationRequested)
                {
                    await Task.WhenAll(Enumerable.Range(0, count).Select(_ => stepCompletedSemaphore.WaitAsync(cts.Token)).ToArray());

                    if (idleCount == count)
                    {
                        consecutiveIdleSteps++;
                        if (natOutput.Count > 0 && consecutiveIdleSteps >= 100)
                        {
                            cts.Cancel();
                        }
                        if (hasNatInput)
                        {
                            natOutput.Add((natInput.Last().x, natInput.Last().y));
                            packetQueues[0].Enqueue((natInput.Last().x, natInput.Last().y));
                            hasNatInput = false;
                        }
                    }
                    else
                    {
                        consecutiveIdleSteps = 0;
                    }

                    idleCount = 0;

                    for (var idx = 0; idx < stepGuards.Length; idx++)
                    {
                        stepGuards[idx].Set();
                    }
                }
            };

            await Task.WhenAll(Task.WhenAll(tasks), NatControllerAsync());
            return natOutput;
        }
    }
}
