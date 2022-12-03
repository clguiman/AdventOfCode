using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2022
{
    public class Day03
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(157, Part1(ParseInput(new[] {
                "vJrwpWtwJgWrhcsFMMfFFhFp",
                "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
                "PmmdzqPrVvPwwTWBwg",
                "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
                "ttgJtRGJQctTZtZT",
                "CrZsJsPPZsGzwwsLwLmpwMDw"
            })));
        }
        [Fact]
        public void Test2()
        {
            Assert.Equal(7785, Part1(ParseInput(File.ReadAllLines("input/day03.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(70, Part2(ParseInput2(new[] {
                "vJrwpWtwJgWrhcsFMMfFFhFp",
                "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
                "PmmdzqPrVvPwwTWBwg",
                "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
                "ttgJtRGJQctTZtZT",
                "CrZsJsPPZsGzwwsLwLmpwMDw"
            })));
        }
        [Fact]
        public void Test4()
        {
            Assert.Equal(2633, Part2(ParseInput2(File.ReadAllLines("input/day03.txt"))));
        }

        private static int Part1(IEnumerable<(int[] comp1, int[] comp2)> rucksacks) => rucksacks.Select(x => x.comp1.Intersect(x.comp2).First()).Sum();

        private static int Part2(IEnumerable<(int[] rucksack1, int[] rucksack2, int[] rucksack3)> rucksacks) => rucksacks.Select(x => x.rucksack1.Intersect(x.rucksack2).Intersect(x.rucksack3).First()).Sum();

        private static IEnumerable<(int[] comp1, int[] comp2)> ParseInput(string[] input)
        {
            var charToInt = (char c) => { return c >= 'a' && c <= 'z' ? c - 'a' + 1 : c - 'A' + 27; };
            foreach (var line in input)
            {
                var first = line.Substring(0, (line.Length ) / 2).Select(charToInt).ToArray();
                var second = line.Substring((line.Length ) / 2).Select(charToInt).ToArray();
                yield return (first, second);
            }
        }

        private static IEnumerable<(int[] rucksack1, int[] rucksack2, int[] rucksack3)> ParseInput2(string[] input)
        {
            var charToInt = (char c) => { return c >= 'a' && c <= 'z' ? c - 'a' + 1 : c - 'A' + 27; };
            for(var i = 0; i < input.Length; i+=3)
            {
                var first = input[i].Select(charToInt).ToArray();
                var second = input[i + 1].Select(charToInt).ToArray();
                var third = input[i + 2].Select(charToInt).ToArray();
                yield return (first, second, third);
            }
        }
    }
}
