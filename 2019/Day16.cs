using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace _2019
{
    public class Day16
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("48226158", SignalAsString(RunFFT(ParseSignal("12345678"), 1)));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("34040438", SignalAsString(RunFFT(ParseSignal("12345678"), 2)));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("03415518", SignalAsString(RunFFT(ParseSignal("12345678"), 3)));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal("01029498", SignalAsString(RunFFT(ParseSignal("12345678"), 4)));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal("24176176", SignalAsString(RunFFT(ParseSignal("80871224585914546619083218645595"), 100)).Substring(0, 8));
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal("30379585", SignalAsString(RunFFT(ParseSignal(File.ReadAllText("input/day16.txt")), 100)).Substring(0, 8));
        }

        [Fact]
        public void Test7()
        {
            Assert.Equal("84462026", SignalAsString(RunFFTPart2(ParseSignal("03036732577212944063491565474664"), 100)).Substring(0, 8));
        }

        [Fact]
        public void Test8()
        {
            Assert.Equal("78725270", SignalAsString(RunFFTPart2(ParseSignal("02935109699940807407585447034323"), 100)).Substring(0, 8));
        }

        [Fact]
        public void Test9()
        {
            Assert.Equal("53553731", SignalAsString(RunFFTPart2(ParseSignal("03081770884921959731165446850517"), 100)).Substring(0, 8));
        }

        [Fact]
        public void Test10()
        {
            Assert.Equal("22808931", SignalAsString(RunFFTPart2(ParseSignal(File.ReadAllText("input/day16.txt")), 100)).Substring(0, 8));
        }

        private static int[] ParseSignal(string input) => input.Select(c => c - '0').ToArray();

        private static string SignalAsString(int[] input)
        {
            StringBuilder sb = new();
            foreach (var n in input)
            {
                sb.Append(n);
            }
            return sb.ToString();
        }

        private int[] RunFFT(int[] signal, int phases)
        {
            int[] ret = signal;
            for (var i = 0; i < phases; i++)
            {
                ret = Enumerable.Range(1, ret.Length)
                                .Select(pos => Math.Abs(ret.Zip(GeneratePattern(pos, ret.Length))
                                                .Select(x => (x.First * x.Second) % 10).Sum() % 10))
                                .ToArray();
            }
            return ret;
        }

        private int[] RunFFTPart2(int[] signal, int phases)
        {
            int messageOffset = 0;
            for (var i = 0; i < 7; i++)
            {
                messageOffset *= 10;
                messageOffset += signal[i];
            }

            int[] ret = new int[signal.Length * 10000 - messageOffset];
            int index = 0;
            for (int i = messageOffset; i < signal.Length * 10000; i++)
            {
                ret[index++] = signal[i % signal.Length];
            }

            for (var i = 0; i < phases; i++)
            {
                for(var j = ret.Length - 2; j >= 0; j--)
                {
                    ret[j]=(ret[j] + ret[j+1]) % 10;
                }
            }

            return ret;
        }

        private static IEnumerable<int> GeneratePattern(int repeatCount, int length)
        {
            int basePos = 0;
            int leftOverCount = repeatCount - 1;
            for (var idx = 0; idx < length; idx++)
            {
                if (leftOverCount == 0)
                {
                    basePos = (basePos + 1) % _basePattern.Length;
                    leftOverCount = repeatCount;
                }
                yield return _basePattern[basePos];

                leftOverCount--;
            }
        }

        private static readonly int[] _basePattern = new int[] { 0, 1, 0, -1 };
    }
}
