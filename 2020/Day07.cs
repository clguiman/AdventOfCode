using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day07
    {
        [Fact]
        public void Test1()
        {
            var input = new[] {
"light red bags contain 1 bright white bag, 2 muted yellow bags.",
"dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
"bright white bags contain 1 shiny gold bag.",
"muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
"shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
"dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
"vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
"faded blue bags contain no other bags.",
"dotted black bags contain no other bags." };
            Assert.Equal(4, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(332, Part1(File.ReadAllLines("input/day07.txt")));
        }

        [Fact]
        public void Test3()
        {
            var input = new[] {
"shiny gold bags contain 2 dark red bags.",
"dark red bags contain 2 dark orange bags.",
"dark orange bags contain 2 dark yellow bags.",
"dark yellow bags contain 2 dark green bags.",
"dark green bags contain 2 dark blue bags.",
"dark blue bags contain 2 dark violet bags.",
"dark violet bags contain no other bags." };
            Assert.Equal(126, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            var input = new[] {
"light red bags contain 1 bright white bag, 2 muted yellow bags.",
"dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
"bright white bags contain 1 shiny gold bag.",
"muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
"shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
"dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
"vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
"faded blue bags contain no other bags.",
"dotted black bags contain no other bags." };
            Assert.Equal(32, Part2(input));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(10875, Part2(File.ReadAllLines("input/day07.txt")));
        }

        private static int Part1(IEnumerable<string> input)
        {
            return ParseBags(input).Sum(b => IsShinyGoldBagInside(b) ? 1 : 0) - 1;
        }

        private static int Part2(IEnumerable<string> input)
        {
            return ComputeContainingBagCount(
                ParseBags(input)
                .Where(b => b.Name == "shiny gold").First(), 1) - 1;
        }

        private static List<Bag> ParseBags(IEnumerable<string> input)
        {
            Dictionary<string, Bag> bagMap = new();
            return input
                .Select(s => s.Replace('.', ','))
                .Select(x =>
                {
                    var t = x.Split("bags contain");
                    var bagName = t[0].Trim();
                    var dependendBags = t[1].Trim().Split(",").Select(b => b.Trim()).Where(b => b.Length > 0)
                    .Where(b => !b.StartsWith("no"))
                    .Select(b =>
                    {
                        var countToken = b.Split(' ')[0];
                        var count = int.Parse(countToken);
                        var name = b.Substring(b.IndexOf(' ') + 1).Split("bag")[0].Trim();
                        if (!bagMap.ContainsKey(name))
                        {
                            bagMap.Add(name, new Bag() { Name = name, ContainingBags = new() });
                        }
                        return (bag: bagMap[name], count);
                    }).ToList();
                    if (!bagMap.ContainsKey(bagName))
                    {
                        bagMap.Add(bagName, new Bag() { Name = bagName, ContainingBags = dependendBags });
                    }
                    else
                    {
                        bagMap[bagName].ContainingBags.AddRange(dependendBags);
                    }
                    return bagMap[bagName];
                }).ToList();
        }

        private static bool IsShinyGoldBagInside(Bag bag) => bag.Name == "shiny gold" || bag.ContainingBags.Any(b => IsShinyGoldBagInside(b.bag));

        private static int ComputeContainingBagCount(Bag bag, int count)
        {
            if (bag.ContainingBags.Count() == 0)
            {
                return count;
            }
            var s = count;
            foreach (var c in bag.ContainingBags)
            {
                s += count * ComputeContainingBagCount(c.bag, c.count);
            }
            return s;
        }

        private class Bag
        {
            public string Name { get; init; }
            public List<(Bag bag, int count)> ContainingBags { get; init; }
        }
    }
}
