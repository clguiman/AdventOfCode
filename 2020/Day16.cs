using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace _2020
{
    public class Day16
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(71, Part1(new[]{
"class: 1-3 or 5-7",
"row: 6-11 or 33-44",
"seat: 13-40 or 45-50",
"your ticket:",
"7,1,14",
"nearby tickets:",
"7,3,47",
"40,4,50",
"55,2,20",
"38,6,12"}));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(19060, Part1(File.ReadAllLines("input/day16.txt")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(156, Part2(new[]{
"departure class: 0-1 or 4-19",
"row: 0-5 or 8-19",
"departure seat: 0-13 or 16-19",
"your ticket:",
"11,12,13",
"nearby tickets:",
"99,1,5",
"3,9,18",
"15,1,5",
"5,14,9"}));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(953713095011, Part2(File.ReadAllLines("input/day16.txt")));
        }

        private static long Part1(IEnumerable<string> input)
        {
            var bag = ParseInput(input);
            return ParseInput(input).NearbyTickets
                .SelectMany(t => t.Numbers
                    .Where(n => bag.Ranges.All(r => !r.Value.IsInRange(n)))
                    .Select(n => (long)n))
                .Sum();
        }

        private static long Part2(IEnumerable<string> input)
        {
            var parsedBag = ParseInput(input);
            var bag = new Bag()
            {
                Ranges = parsedBag.Ranges,
                YourTicket = parsedBag.YourTicket,
                NearbyTickets = parsedBag.NearbyTickets
                    .Where(ticket => ticket.Numbers
                        .All(n => parsedBag.Ranges.Any(r => r.Value.IsInRange(n))))
                    .ToArray()
            };

            var fieldIndeces = bag.Ranges.Keys.ToDictionary(k => k, k => -1);

            foreach(var (fields, index) in Enumerable.Range(0, bag.YourTicket.Numbers.Count())
                .Select(idx =>
                {
                    var validFields = bag.NearbyTickets
                    .Select(ticket =>
                        bag.Ranges
                            .Where(r => r.Value.IsInRange(ticket.Numbers.Skip(idx).First()))
                            .Select(x => (name: x.Key, range: x.Value))
                        )
                    .Aggregate((x, y) => x.Intersect(y));

                    return (fields: validFields.Select(x => x.name), index: idx);
                })
                .OrderBy(x => x.fields.Count()))
            {
                foreach(var name in fields)
                {
                    if (fieldIndeces[name] == -1)
                    {
                        fieldIndeces[name] = index;
                        break;
                    }
                }
            }

            return fieldIndeces
                .Where(x => x.Key.StartsWith("departure") && x.Value != -1)
                .Select(x => (long)bag.YourTicket.Numbers.Skip(x.Value).First())
                .Aggregate(1L, (a, b) => a * b);
        }

        private static Bag ParseInput(IEnumerable<string> input)
        {
            Dictionary<string, ValidRanges> ranges = new();
            Ticket yourTicket = null;
            List<Ticket> nearbyTickets = null;

            bool parsingTickets = false;
            bool parsingNearbyTickets = false;

            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (line.StartsWith("your ticket:"))
                {
                    parsingTickets = true;
                    parsingNearbyTickets = false;
                    continue;
                }
                if (line.StartsWith("nearby tickets:"))
                {
                    parsingTickets = true;
                    parsingNearbyTickets = true;
                    nearbyTickets = new();
                    continue;
                }
                if (!parsingTickets)
                {
                    var tokens = line.Split(':');
                    ranges.Add(tokens[0].Trim(), ValidRanges.FromString(tokens[1].Trim()));
                    continue;
                }

                if (parsingTickets && !parsingNearbyTickets)
                {
                    yourTicket = Ticket.FromString(line);
                    continue;
                }

                if (parsingTickets && parsingNearbyTickets)
                {
                    nearbyTickets.Add(Ticket.FromString(line));
                    continue;
                }

                throw new Exception("invalid line");
            }

            return new Bag() { Ranges = ranges, YourTicket = yourTicket, NearbyTickets = nearbyTickets };
        }

        private record Bag
        {
            public Dictionary<string, ValidRanges> Ranges { get; init; }

            public Ticket YourTicket { get; init; }

            public IEnumerable<Ticket> NearbyTickets { get; init; }
        }

        private class Ticket
        {
            public IEnumerable<int> Numbers { get; init; }

            public static Ticket FromString(string str) => new Ticket()
            {
                Numbers = str.Split(',').Select(int.Parse).ToArray()
            };

            public override string ToString()
            {
                StringBuilder sb = new(Numbers.Count() * 3);
                foreach (var n in Numbers)
                {
                    sb.Append(n);
                    sb.Append(' ');
                }
                return sb.ToString();
            }
        }

        private class ValidRanges
        {
            public ValidRanges(int firstRangeStart, int firstRangeEnd, int secondRangeStart, int secondRangeEnd)
            {
                ranges = new[] { (firstRangeStart, firstRangeEnd), (secondRangeStart, secondRangeEnd) };
            }

            public static ValidRanges FromString(string str)
            {
                var tokens = str.Split("or");
                var firstStr = tokens[0].Trim().Split('-');
                var secondStr = tokens[1].Trim().Split('-');
                return new ValidRanges(int.Parse(firstStr[0]), int.Parse(firstStr[1]), int.Parse(secondStr[0]), int.Parse(secondStr[1]));
            }

            public bool IsInRange(int x) => ranges.Any(item => item.Item1 <= x && item.Item2 >= x);

            public override string ToString() => $"{ranges[0].Item1}-{ranges[0].Item2} or {ranges[1].Item1}-{ranges[1].Item2}";

            private readonly (int, int)[] ranges;
        }
    }
}
