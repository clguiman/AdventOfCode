using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day14
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
"mem[8] = 11",
"mem[7] = 101",
"mem[8] = 0", };
            Assert.Equal(165, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(15919415426101, Part1(File.ReadAllLines("input/day14.txt")));
        }

        [Fact]
        public void Test3()
        {
            var input = new[] { "mask = 000000000000000000000000000000X1001X",
"mem[42] = 100",
"mask = 00000000000000000000000000000000X0XX",
"mem[26] = 1" };
            Assert.Equal(208, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(3443997590975, Part2(File.ReadAllLines("input/day14.txt")));
        }

        private static long Part1(IEnumerable<string> input)
        {
            long AndMask = -1L;
            long OrMask = 0L;
            Dictionary<long, long> memory = new();
            foreach (var entry in input)
            {
                if (entry.StartsWith("mask"))
                {
                    AndMask = 0L;
                    OrMask = 0L;
                    foreach (var maskBit in entry.Split('=').Last().Trim())
                    {
                        OrMask = OrMask * 2 + ((maskBit == '1') ? 1 : 0);
                        AndMask = AndMask * 2 + ((maskBit == '0') ? 0 : 1);
                    }
                    continue;
                }
                if (entry.StartsWith("mem"))
                {
                    var address = long.Parse(entry[4..].Split(']')[0]);
                    var value = (long.Parse(entry.Split('=').Last().Trim()) & AndMask) | OrMask;
                    if (!memory.TryAdd(address, value))
                    {
                        memory[address] = value;
                    }
                }
                else
                {
                    throw new Exception("invalid entry!");
                }
            }
            return memory.Values.Sum();
        }

        private static long Part2(IEnumerable<string> input)
        {
            long FloatingMask = 0L;
            long OrMask = 0L;
            Dictionary<long, long> memory = new();
            foreach (var entry in input)
            {
                if (entry.StartsWith("mask"))
                {
                    FloatingMask = 0L;
                    OrMask = 0L;
                    foreach (var maskBit in entry.Split('=').Last().Trim())
                    {
                        FloatingMask = FloatingMask * 2 + ((maskBit == 'X') ? 1 : 0);
                        OrMask = OrMask * 2 + ((maskBit == '1') ? 1 : 0);
                    }
                    continue;
                }
                if (entry.StartsWith("mem"))
                {
                    var originalAddress = long.Parse(entry.Substring(4).Split(']')[0]);
                    var value = long.Parse(entry.Split('=').Last().Trim());
                    originalAddress |= OrMask;
                    foreach (var address in GenerateAllMasksFromFloatingMask(FloatingMask).Select(m => (originalAddress & m.AndMask) | (m.OrMask)))
                    {
                        if (!memory.TryAdd(address, value))
                        {
                            memory[address] = value;
                        }
                    }
                }
                else
                {
                    throw new Exception("invalid entry!");
                }
            }
            return memory.Values.Sum();
        }

        private static IEnumerable<(long AndMask, long OrMask)> GenerateAllMasksFromFloatingMask(long floatingMask)
        {
            List<(long AndMask, long OrMask)> computedMasks = new() { (AndMask: -1L, OrMask: 0L) };
            for (var bitMask = 1L; floatingMask > 0; floatingMask >>= 1, bitMask <<= 1)
            {
                if (floatingMask % 2 == 0)
                {
                    continue;
                }
                computedMasks = computedMasks
                    .SelectMany(c => new[] {
                        (c.AndMask & ~bitMask, c.OrMask ),
                        (c.AndMask, c.OrMask | bitMask) })
                    .ToList();
            }
            return computedMasks;
        }
    }
}
