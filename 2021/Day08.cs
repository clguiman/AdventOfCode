using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day08
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(375, File.ReadAllLines("input/day08.txt")
                .Select(l => l.Split('|')[1].Trim())
                .SelectMany(o => o.Split(' '))
                .Count(x => (x.Length >= 2 && x.Length <= 4) || x.Length == 7));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(5353, Part2(
                new[]
                {
                    new[]
                    {
                        "acedgfb", "cdfbe", "gcdfa", "fbcad", "dab", "cefabd", "cdfgeb", "eafb", "cagedb", "ab"
                    }
                },
                new[]
                {
                    new[]
                    {
                        "cdfeb", "fcadb", "cdfeb", "cdbaf"
                    }
                }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(7322, Part2(
                new[]
                {
                    new[]
                    {
                        "degb", "cfagedb", "fgdea", "fgabc", "eb", "fegadc", "dfaceb", "eab", "gbefa", "dgaebf"
                    }
                },
                new[]
                {
                    new[]
                    {
                        "aeb", "bgeaf", "bfagc", "fbagc"
                    }
                }));
        }

        [Fact]
        public void Test5()
        {
            var input = File.ReadAllLines("input/day08.txt");
            var signals = new List<IEnumerable<string>>();
            var outputs = new List<IEnumerable<string>>();
            foreach (var line in input)
            {
                var s = line.Split('|');
                signals.Add(s[0].Trim().Split(' '));
                outputs.Add(s[1].Trim().Split(' '));
            }

            Assert.Equal(1019355, Part2(signals.Select(s => s.ToArray()).ToArray(), outputs.Select(o => o.ToArray()).ToArray()));
        }

        private static int Part2(string[][] signals, string[][] outputs) =>
            Enumerable.Range(0, signals.Length)
            .Select(idx => Solve(signals[idx], outputs[idx]))
            .Sum();

        private static int Solve(string[] signal, string[] output)
        {
            var segments = new[] { ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
            /*
             * segments:
                 0000
                5    1
                5    1
                 6666 
                4    2
                4    2
                 3333
             */
            var one = signal.Where(x => x.Length == 2).FirstOrDefault();
            var seven = signal.Where(x => x.Length == 3).FirstOrDefault();
            var four = signal.Where(x => x.Length == 4).FirstOrDefault();
            var eight = signal.Where(x => x.Length == 7).FirstOrDefault();
            segments[0] = seven.Except(one).FirstOrDefault();

            var zeroOrNine = signal.Where(x => x.Length == 6).Where(x => x.Contains(one[0]) && x.Contains(one[1])).ToArray();
            if (four.Contains(zeroOrNine[0].Except(zeroOrNine[1]).FirstOrDefault()))
            {
                var nine = zeroOrNine[0];
                var zero = zeroOrNine[1];
                segments[6] = nine.Except(zero).FirstOrDefault();
                segments[5] = four.Except(one).Where(x => x != segments[6]).FirstOrDefault();
                segments[4] = zero.Except(nine).FirstOrDefault();
                segments[3] = zero.Except(four).Where(x => x != segments[0] && x != segments[4]).FirstOrDefault();
                var two = signal.Where(x => x.Length == 5).Where(x => x.Contains(segments[0]) && x.Contains(segments[4])).FirstOrDefault();
                segments[1] = two.Intersect(one).FirstOrDefault();
                segments[2] = one.Where(x => x != segments[1]).FirstOrDefault();
            }
            else
            {
                segments[4] = zeroOrNine[0].Except(zeroOrNine[1]).FirstOrDefault();
                var two = signal.Where(x => x.Length == 5).Where(x => x.Contains(segments[4])).FirstOrDefault();
                segments[1] = two.Intersect(one).FirstOrDefault();
                segments[2] = one.Where(x => x != segments[1]).FirstOrDefault();
                segments[6] = two.Intersect(four).Where(x => x != segments[1]).FirstOrDefault();
                segments[3] = two.Where(x => x != segments[0] && x != segments[1] && x != segments[6] && x != segments[4]).FirstOrDefault();
                segments[5] = eight.Where(x => x != segments[0] && x != segments[1] && x != segments[2] && x != segments[3] && x != segments[4] && x != segments[6]).FirstOrDefault();
            }

            var charToSegment = new Dictionary<char, int>();
            for (var idx = 0; idx < segments.Length; idx++)
            {
                charToSegment[segments[idx]] = idx;
            }

            return int.Parse(string.Concat(output
                .Select(digit => string.Concat(digit
                    .Select(x => charToSegment[x])
                    .OrderBy(x => x)
                    .Select(x => (char)('0' + x))))
                .Select(d => SegmentsToDigitMap[d])
                .Select(x => (char)('0' + x))));
        }

        private static readonly Dictionary<string, int> SegmentsToDigitMap = new Dictionary<string, int>
        {
            { "012345", 0 },
            { "12", 1 },
            { "01346", 2 },
            { "01236", 3 },
            { "1256", 4 },
            { "02356", 5 },
            { "023456", 6 },
            { "012", 7 },
            { "0123456", 8 },
            { "012356", 9 }
        };
    }
}
