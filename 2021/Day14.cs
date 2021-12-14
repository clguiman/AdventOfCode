using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day14
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(1588, Solve(ParseInput(new[] {
                "NNCB",
                "",
                "CH -> B",
                "HH -> N",
                "CB -> H",
                "NH -> C",
                "HB -> C",
                "HC -> B",
                "HN -> C",
                "NN -> C",
                "BH -> H",
                "NC -> B",
                "NB -> B",
                "BN -> B",
                "BB -> N",
                "BC -> B",
                "CC -> N",
                "CN -> C"
            }), 10));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(2602, Solve(ParseInput(File.ReadAllLines("input/day14.txt")), 10));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(2188189693529, Solve(ParseInput(new[] {
                "NNCB",
                "",
                "CH -> B",
                "HH -> N",
                "CB -> H",
                "NH -> C",
                "HB -> C",
                "HC -> B",
                "HN -> C",
                "NN -> C",
                "BH -> H",
                "NC -> B",
                "NB -> B",
                "BN -> B",
                "BB -> N",
                "BC -> B",
                "CC -> N",
                "CN -> C"
            }), 40));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(2942885922173, Solve(ParseInput(File.ReadAllLines("input/day14.txt")), 40));
        }

        private static long Solve((string template, Dictionary<string, char> pairs) input, int steps)
        {
            var pairOccurences = new Dictionary<string, long>();
            for (var i = 0; i < input.template.Length - 1; i++)
            {
                var curPair = string.Concat(input.template[i], input.template[i + 1]);
                if (pairOccurences.ContainsKey(curPair))
                {
                    pairOccurences[curPair]++;
                }
                else
                {
                    pairOccurences[curPair] = 1;
                }
            }
            var pairs = input.pairs;

            for (var step = 0; step < steps; step++)
            {
                var newPairOccurences = new Dictionary<string, long>();
                foreach (var occurence in pairOccurences)
                {
                    var curPair = occurence.Key;
                    if (!pairs.TryGetValue(curPair, out var charToInsert))
                    {
                        continue;
                    }

                    var firstChar = curPair[0];
                    var secondChar = curPair[1];
                    foreach (var newPair in new[] { string.Concat(firstChar, charToInsert), string.Concat(charToInsert, secondChar) })
                    {
                        if (newPairOccurences.ContainsKey(newPair))
                        {
                            newPairOccurences[newPair] += occurence.Value;
                        }
                        else
                        {
                            newPairOccurences[newPair] = occurence.Value;
                        }
                    }
                }

                pairOccurences = newPairOccurences;
            }

            var occurenceCount = new long['Z' - 'A' + 1];
            foreach (var occurence in pairOccurences)
            {
                occurenceCount[occurence.Key[0] - 'A'] += occurence.Value;
            }
            occurenceCount[input.template.Last() - 'A']++;

            var orderedElements = occurenceCount.OrderByDescending(x => x).Where(x => x != 0).ToArray();
            return orderedElements.First() - orderedElements.Last();
        }

        private static (string, Dictionary<string, char>) ParseInput(IEnumerable<string> input)
        {
            var template = string.Empty;
            var pairs = new Dictionary<string, char>();
            foreach (var line in input)
            {

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(template))
                {
                    template = line;
                    continue;
                }
                var s = line.Split("->");
                pairs.Add(s[0].Trim(), s[1].Trim()[0]);

            }
            return (template, pairs);
        }
    }
}
