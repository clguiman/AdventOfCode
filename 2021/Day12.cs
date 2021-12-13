using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2021
{
    public class Day12
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(10, Part1(new[] {
                "start-A",
                "start-b",
                "A-c",
                "A-b",
                "b-d",
                "A-end",
                "b-end" }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(226, Part1(new[] {
                "fs-end",
                "he-DX",
                "fs-he",
                "start-DX",
                "pj-DX",
                "end-zg",
                "zg-sl",
                "zg-pj",
                "pj-he",
                "RW-he",
                "fs-DX",
                "pj-RW",
                "zg-RW",
                "start-pj",
                "he-WI",
                "zg-he",
                "pj-fs",
                "start-RW" }));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(4691, Part1(File.ReadAllLines("input/day12.txt")));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(36, Part2(new[] {
                "start-A",
                "start-b",
                "A-c",
                "A-b",
                "b-d",
                "A-end",
                "b-end" }));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(3509, Part2(new[] {
                "fs-end",
                "he-DX",
                "fs-he",
                "start-DX",
                "pj-DX",
                "end-zg",
                "zg-sl",
                "zg-pj",
                "pj-he",
                "RW-he",
                "fs-DX",
                "pj-RW",
                "zg-RW",
                "start-pj",
                "he-WI",
                "zg-he",
                "pj-fs",
                "start-RW" }));
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal(140718, Part2(File.ReadAllLines("input/day12.txt")));
        }

        private static int Part1(IEnumerable<string> input) => Graph<string>.AsDirected(
            input.Select(x => { var s = x.Split('-'); return (s[0], s[1]); }))
            .DFS<OrderedPath<string,bool>, bool>("start", "end",
                shouldWalkPredicate: x => x.possibleAdjacentItem != "start",
                shouldReWalkPredicate: x => string.Equals(x.possibleAdjacentItem, x.possibleAdjacentItem.ToUpper()))
            .Count();

        private static int Part2(IEnumerable<string> input)
        {
            //return FindAllPathsToEnd(graph, "start", new(), true);
            return Graph<string>.AsDirected(
            input.Select(x => { var s = x.Split('-'); return (s[0], s[1]); }))
            .DFS<UnOrderedPath<string, bool>, bool>("start", "end",
                shouldWalkPredicate: x => x.possibleAdjacentItem != "start",
                shouldReWalkPredicate: (x) =>
                {
                    if(string.Equals(x.possibleAdjacentItem, x.possibleAdjacentItem.ToLower()))
                    {
                        if( x.currentPath.UserContext == false)
                        {
                            x.currentPath.UserContext = true;
                            return true;
                        }
                        return false;
                    }
                    return true;
                })
            .Count();
        }

        private static int FindAllPathsToEnd(Dictionary<string, HashSet<string>> graph, string start, HashSet<string> visitedSmallCaves, bool allowVisitTwice = false)
        {
            if (start == "end")
            {
                return 1;
            }
            var total = 0;
            var nextDestinations = graph[start].Where(x => x != "start");
            foreach (var next in nextDestinations)
            {
                var nextCanVisitTwice = allowVisitTwice;
                if (string.Equals(next, next.ToLower()) && visitedSmallCaves.Contains(next))
                {
                    if (!nextCanVisitTwice)
                    {
                        continue;
                    }
                    nextCanVisitTwice = false;
                }

                var visitedCaves = visitedSmallCaves;
                if (string.Equals(start, start.ToLower()))
                {
                    visitedCaves = visitedSmallCaves.Concat(new[] { start }).ToHashSet();
                }
                total += FindAllPathsToEnd(graph, next, visitedCaves, nextCanVisitTwice);
            }
            return total;
        }

        private static Dictionary<string, HashSet<string>> ParseInput(IEnumerable<string> input)
        {
            var graph = Graph<string>.AsDirected(input.Select(x => { var s = x.Split('-'); return (s[0], s[1]); }));
            var ret = new Dictionary<string, HashSet<string>>();

            foreach (var edge in input.Select(x => { var s = x.Split('-'); return (s[0], s[1]); }))
            {
                if (ret.ContainsKey(edge.Item1))
                {
                    ret[edge.Item1].Add(edge.Item2);
                }
                else
                {
                    ret.Add(edge.Item1, new() { edge.Item2 });
                }

                if (ret.ContainsKey(edge.Item2))
                {
                    ret[edge.Item2].Add(edge.Item1);
                }
                else
                {
                    ret.Add(edge.Item2, new() { edge.Item1 });
                }
            }
            return ret;
        }
    }
}
