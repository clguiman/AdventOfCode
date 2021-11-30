using System;
using System.Collections.Generic;
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

        private static int[] ParseSignal(string input)
        {
            int[] ret = new int[input.Length];
            var idx = 0;
            foreach (var c in input)
            {
                ret[idx] = c - '0';
                idx++;
            }
            return ret;
        }

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
                                .Select(pos => Math.Abs(ret.Zip(GeneratePattern(pos, signal.Length))
                                                .Select(x => (x.First * x.Second) % 10).Sum() % 10))
                                .ToArray();
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
