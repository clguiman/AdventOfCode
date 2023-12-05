using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2023
{
    public class Day05
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(35LU, Solve(ParseInput([
            "seeds: 79 14 55 13",
            "",
            "seed-to-soil map:",
            "50 98 2",
            "52 50 48",
            "",
            "soil-to-fertilizer map:",
            "0 15 37",
            "37 52 2",
            "39 0 15",
            "",
            "fertilizer-to-water map:",
            "49 53 8",
            "0 11 42",
            "42 0 7",
            "57 7 4",
            "",
            "water-to-light map:",
            "88 18 7",
            "18 25 70",
            "",
            "light-to-temperature map:",
            "45 77 23",
            "81 45 19",
            "68 64 13",
            "",
            "temperature-to-humidity map:",
            "0 69 1",
            "1 0 69",
            "",
            "humidity-to-location map:",
            "60 56 37",
            "56 93 4"
                ], true)));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(388071289LU, Solve(ParseInput(File.ReadAllLines("input/day05.txt"), true)));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(46LU, Solve(ParseInput([
            "seeds: 79 14 55 13",
            "",
            "seed-to-soil map:",
            "50 98 2",
            "52 50 48",
            "",
            "soil-to-fertilizer map:",
            "0 15 37",
            "37 52 2",
            "39 0 15",
            "",
            "fertilizer-to-water map:",
            "49 53 8",
            "0 11 42",
            "42 0 7",
            "57 7 4",
            "",
            "water-to-light map:",
            "88 18 7",
            "18 25 70",
            "",
            "light-to-temperature map:",
            "45 77 23",
            "81 45 19",
            "68 64 13",
            "",
            "temperature-to-humidity map:",
            "0 69 1",
            "1 0 69",
            "",
            "humidity-to-location map:",
            "60 56 37",
            "56 93 4"
                ], false)));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(84206669LU, Solve(ParseInput(File.ReadAllLines("input/day05.txt"), false)));
        }

        private static UInt64 Solve((List<SeedRange> seeds, List<Map> maps) input)
        {
            var seeds = input.seeds.ToList();
            foreach (var map in input.maps)
            {
                List<SeedRange> transformedSeeds = [];
                for (var idx = 0; idx < seeds.Count; idx++)
                {
                    foreach (var entry in map.Entries)
                    {
                        var (transformed, unchanged) = seeds[idx].ApplyMap(entry, out var hasChanged);
                        if (hasChanged)
                        {
                            seeds.RemoveAt(idx);
                            idx--;
                            transformedSeeds.AddRange(transformed);
                            seeds.AddRange(unchanged);
                            break;
                        }
                    }
                }
                seeds.AddRange(transformedSeeds);
            }

            return seeds.Select(x => x.Start).Min();
        }

        private static (List<SeedRange> seeds, List<Map> maps) ParseInput(IEnumerable<string> input, bool isPart1)
        {
            var inputEnumerator = input.GetEnumerator(); inputEnumerator.MoveNext();
            var seedsLine = inputEnumerator.Current;
            List<SeedRange> seeds;
            if (isPart1)
            {
                seeds = seedsLine.Split(':')[1].Trim().Split(' ')
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(UInt64.Parse)
                        .Select(x => new SeedRange(x, x))
                        .ToList();
            }
            else
            {
                seeds = seedsLine.Split(':')[1].Trim().Split(' ')
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(UInt64.Parse)
                        .Select((val, idx) => (val, idx))
                        .GroupBy(x => x.idx / 2)
                        .Select(x =>
                        {
                            var t = x.ToArray();
                            return new SeedRange(t[0].val, t[0].val + t[1].val - 1);
                        })
                        .ToList();
            }

            List<Map> maps = [];
            inputEnumerator.MoveNext();// jump to map;

            while (inputEnumerator.MoveNext())
            {
                var line = inputEnumerator.Current.Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.EndsWith("map:", StringComparison.Ordinal))
                {
                    maps.Add(new Map(line.Split(' ')[0], []));
                    continue;
                }
                var mapEntryValues = line.Split(' ').Select(UInt64.Parse).ToArray();
                maps[^1].Entries.Add(new MapEntry(mapEntryValues[0], mapEntryValues[1], mapEntryValues[2]));
            }

            return (seeds, maps);
        }

        private record SeedRange(UInt64 Start, UInt64 End)
        {
            public (List<SeedRange> transformed, List<SeedRange> unchanged) ApplyMap(MapEntry mapEntry, out bool hasChanged)
            {
                hasChanged = false;
                var mapStart = mapEntry.SrcRangeStart;
                var mapEnd = mapEntry.SrcRangeStart + mapEntry.RangeLength - 1;
                if (Start > mapEnd)
                {
                    return EmptyTransform;
                }
                if (Start >= mapStart)
                {
                    hasChanged = true;
                    List<SeedRange> unchanged = [];
                    if (End > mapEnd)
                    {
                        unchanged.Add(new SeedRange(mapEnd + 1, End));
                    }
                    return ([new SeedRange(mapEntry.Transform(Start), mapEntry.Transform(Math.Min(End, mapEnd)))], unchanged);
                }
                if (End < mapStart)
                {
                    return EmptyTransform;
                }
                if (End <= mapEnd)
                {
                    hasChanged = true;
                    return (transformed: [new SeedRange(mapEntry.DestRangeStart, mapEntry.Transform(End))], unchanged: [new SeedRange(Start, mapStart - 1)]);
                }

                // Start < mapStart && End > mapEnd
                hasChanged = true;
                return (transformed: [new SeedRange(mapEntry.DestRangeStart, mapEntry.Transform(mapEnd))], unchanged: [new SeedRange(Start, mapStart - 1), new SeedRange(mapEnd + 1, End)]);
            }

            private static readonly (List<SeedRange> transformed, List<SeedRange> unchanged) EmptyTransform = ([], []);
        }

        private record MapEntry(UInt64 DestRangeStart, UInt64 SrcRangeStart, UInt64 RangeLength)
        {
            public UInt64 Transform(UInt64 val)
            {
                if (DestRangeStart > SrcRangeStart)
                {
                    var diff = DestRangeStart - SrcRangeStart;
                    return val + diff;
                }
                else
                {
                    var diff = SrcRangeStart - DestRangeStart;
                    return val - diff;
                }
            }
        }

        private record Map(string Name, List<MapEntry> Entries);
    }
}
