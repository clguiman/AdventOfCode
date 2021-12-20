using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2018
{
    public class Day02
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(12, Part1(new[] { "abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab" }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(7533, Part1(File.ReadAllLines("input/day02.txt")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("fgij", Part2(new[] { "abcde", "fghij", "klmno", "pqrst", "fguij", "axcye", "wvxyz" }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal("mphcuasvrnjzzkbgdtqeoylva", Part2(File.ReadAllLines("input/day02.txt")));
        }

        public static int Part1(IEnumerable<string> input)
        {
            var twiceCount = 0;
            var thriceCount = 0;
            foreach (var boxId in input)
            {
                Dictionary<char, int> occurenceCount = new();
                foreach (var c in boxId)
                {
                    if (occurenceCount.ContainsKey(c))
                    {
                        occurenceCount[c]++;
                    }
                    else
                    {
                        occurenceCount.Add(c, 1);
                    }
                }
                if (occurenceCount.Values.Any(x => x == 2))
                {
                    twiceCount++;
                }
                if (occurenceCount.Values.Any(x => x == 3))
                {
                    thriceCount++;
                }
            }
            return twiceCount * thriceCount;
        }

        public static string Part2(string[] input)
        {

            for (var i = 0; i < input.Length - 1; i++)
            {
                for (var j = i + 1; j < input.Length; j++)
                {
                    var s1 = input[i];
                    var s2 = input[j];

                    for (var idx = 0; idx < s1.Length; idx++)
                    {
                        if (s1[idx] != s2[idx])
                        {
                            if (string.Equals(s1[(idx + 1)..], s2[(idx + 1)..]))
                            {
                                return string.Concat(s1[..idx], s1[(idx + 1)..]);
                            }
                            break;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
