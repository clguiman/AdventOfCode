using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace _2019
{
    public class Day14
    {
        [Fact]
        public void Test1()
        {
            var input = new[] {
                "10 ORE => 10 A",
                "1 ORE => 1 B",
                "7 A, 1 B => 1 C",
                "7 A, 1 C => 1 D",
                "7 A, 1 D => 1 E",
                "7 A, 1 E => 1 FUEL" };
            Assert.Equal(31, Part1(ParseReactionsFromInput(input)));
        }

        [Fact]
        public void Test2()
        {
            var input = new[] {
                "9 ORE => 2 A",
                "8 ORE => 3 B",
                "7 ORE => 5 C",
                "3 A, 4 B => 1 AB",
                "5 B, 7 C => 1 BC",
                "4 C, 1 A => 1 CA",
                "2 AB, 3 BC, 4 CA => 1 FUEL"};
            Assert.Equal(165, Part1(ParseReactionsFromInput(input)));
        }

        [Fact]
        public void Test3()
        {
            var input = new[] {
                "171 ORE => 8 CNZTR",
                "7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL",
                "114 ORE => 4 BHXH",
                "14 VRPVC => 6 BMBT",
                "6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL",
                "6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT",
                "15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW",
                "13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW",
                "5 BMBT => 4 WPTQ",
                "189 ORE => 9 KTJDG",
                "1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP",
                "12 VRPVC, 27 CNZTR => 2 XDBXC",
                "15 KTJDG, 12 BHXH => 5 XCVML",
                "3 BHXH, 2 VRPVC => 7 MZWV",
                "121 ORE => 7 VRPVC",
                "7 XCVML => 6 RJRHP",
                "5 BHXH, 4 VRPVC => 5 LTCX"};
            Assert.Equal(2210736, Part1(ParseReactionsFromInput(input)));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(892207, Part1(ParseReactionsFromInput(File.ReadAllLines("input/day14.txt"))));
        }

        [Fact]
        public void Test5()
        {
            var input = new[] {
               "2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG",
                "17 NVRVD, 3 JNWZP => 8 VPVL",
                "53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL",
                "22 VJHF, 37 MNCFX => 5 FWMGM",
                "139 ORE => 4 NVRVD",
                "144 ORE => 7 JNWZP",
                "5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC",
                "5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV",
                "145 ORE => 6 MNCFX",
                "1 NVRVD => 8 CXFTF",
                "1 VJHF, 6 MNCFX => 4 RFSQX",
                "176 ORE => 6 VJHF"};
            Assert.Equal(5586022, Part2(ParseReactionsFromInput(input)));
        }

        [Fact]
        public void Test6()
        {
            var input = new[] {
                "171 ORE => 8 CNZTR",
                "7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL",
                "114 ORE => 4 BHXH",
                "14 VRPVC => 6 BMBT",
                "6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL",
                "6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT",
                "15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW",
                "13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW",
                "5 BMBT => 4 WPTQ",
                "189 ORE => 9 KTJDG",
                "1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP",
                "12 VRPVC, 27 CNZTR => 2 XDBXC",
                "15 KTJDG, 12 BHXH => 5 XCVML",
                "3 BHXH, 2 VRPVC => 7 MZWV",
                "121 ORE => 7 VRPVC",
                "7 XCVML => 6 RJRHP",
                "5 BHXH, 4 VRPVC => 5 LTCX"};
            Assert.Equal(460664, Part2(ParseReactionsFromInput(input)));
        }

        [Fact]
        public void Test7()
        {
            Assert.Equal(1935267, Part2(ParseReactionsFromInput(File.ReadAllLines("input/day14.txt"))));
        }

        private static long Part1(ChemistryBag bag)
        {
            ResolveReaction(bag.KnownReactions.First(r => r.Result.Element == Element.Fuel), 1, bag);
            return Element.Ore.OwnedUnits;
        }

        private static long Part2(ChemistryBag bag)
        {
            var availableOre = 1000_000_000_000L;
            var producedFuel = 0L;
            var fuelUnits = ClosestPowerOfTwo(availableOre / Part1(bag.Clone()));
            var fuelProducingReaction = bag.KnownReactions.First(r => r.Result.Element == Element.Fuel);

            while (availableOre > 0)
            {
                Element.Ore.Reset(); Element.Fuel.Reset();
                var clonedBag = bag.Clone();
                ResolveReaction(fuelProducingReaction, fuelUnits, bag);
                if (availableOre < Element.Ore.OwnedUnits && fuelUnits > 1)
                {
                    bag = clonedBag;
                    fuelUnits /= 2;
                    continue;
                }
                availableOre -= Element.Ore.OwnedUnits;
                if (availableOre >= 0)
                {
                    producedFuel += fuelUnits;
                }
            }

            return producedFuel;
        }

        private static void ResolveReaction(Reaction reaction, long outputUnits, ChemistryBag bag)
        {
            var multiplyFactor = RoundDivisionUp(outputUnits, reaction.Result.Units);
            foreach (var factor in reaction.Required)
            {
                if (factor.Element == Element.Ore)
                {
                    if (outputUnits > 0)
                    {
                        Element.Ore.ConsumedUnits += factor.Units * multiplyFactor;
                        Element.Ore.OwnedUnits = Element.Ore.ConsumedUnits;
                    }
                    continue;
                }

                var requiredUnits = factor.Units * multiplyFactor;
                var leftoverUnits = factor.Element.OwnedUnits - factor.Element.ConsumedUnits;
                factor.Element.ConsumedUnits += requiredUnits;
                if (leftoverUnits > requiredUnits)
                {
                    continue;
                }

                ResolveReaction(bag.KnownReactions
                                .Where(r => r.Result.Element == factor.Element)
                                .OrderBy(r => Math.Abs(r.Result.Units - requiredUnits + leftoverUnits))
                                .First(),
                                requiredUnits - leftoverUnits, bag);
            }
            reaction.Result.Element.OwnedUnits += reaction.Result.Units * multiplyFactor;
            reaction.Result.Element.Reduce();
        }

        private static long RoundDivisionUp(long dividend, long divisor)
        {
            return dividend / divisor + (dividend % divisor > 0 ? 1 : 0);
        }

        private static long ClosestPowerOfTwo(long x)
        {
            return 1 << (int)Math.Log2((double)x);
        }

        private static ChemistryBag ParseReactionsFromInput(IEnumerable<string> input)
        {
            var elements = new HashSet<Element> { Element.Ore, Element.Fuel };
            Element.Ore.Reset(); Element.Fuel.Reset();
            return new()
            {
                KnownElements = elements,
                KnownReactions = input.Select(x =>
                {
                    var tokens1 = x.Split("=>");
                    var lhs = tokens1[0];
                    var rhs = tokens1[1];
                    return new Reaction() { Required = ParseElements(lhs, elements).ToArray(), Result = ParseElements(rhs, elements).First() };
                }).ToList()
            };
        }

        private static IEnumerable<(Element Element, int Units)> ParseElements(string list, HashSet<Element> elements) =>
            list.Split(',')
                .Select(x => x.Trim())
                .Select(x =>
                {
                    var tokens = x.Split(' ');
                    var units = int.Parse(tokens[0]);
                    var element = new Element() { Name = tokens[1] };
                    if (elements.TryGetValue(element, out var e))
                    {
                        element = e;
                    }
                    else
                    {
                        elements.Add(element);
                    }
                    return (element, units);
                });

        private class Element : IEquatable<Element>
        {
            public string Name { get; init; }
            public long OwnedUnits { get; set; }

            public long ConsumedUnits { get; set; }

            public Element Clone() => new() { Name = this.Name, OwnedUnits = this.OwnedUnits, ConsumedUnits = this.ConsumedUnits };

            public void Reset() { OwnedUnits = 0; ConsumedUnits = 0; }

            public void Reduce() { OwnedUnits -= ConsumedUnits; ConsumedUnits = 0; }

            public bool Equals(Element other) => Name == other.Name;

            public override bool Equals(object obj) => Equals(obj as Element);

            public static bool operator ==(Element a, Element b) => a.Equals(b);

            public static bool operator !=(Element a, Element b) => !a.Equals(b);

            public override int GetHashCode() => Name.GetHashCode();

            public override string ToString() => $"{Name} Owned:{OwnedUnits} Consumed:{ConsumedUnits}";

            public static readonly Element Ore = new() { Name = "ORE" };

            public static readonly Element Fuel = new() { Name = "FUEL" };
        }

        private class Reaction
        {
            public IEnumerable<(Element Element, int Units)> Required { get; init; }
            public (Element Element, int Units) Result { get; init; }

            public override string ToString()
            {
                StringBuilder sb = new(256);
                foreach (var r in Required)
                {
                    sb.Append($"{r.Units} {r.Element.Name} ");
                }
                sb.Append("=> ");
                sb.Append($"{Result.Units} {Result.Element.Name}");
                return sb.ToString();
            }
        }

        private class ChemistryBag
        {
            public IReadOnlySet<Element> KnownElements { get; init; }
            public IEnumerable<Reaction> KnownReactions { get; init; }

            public ChemistryBag Clone() => new()
            {
                KnownReactions = this.KnownReactions,
                KnownElements = this.KnownElements
                    .Where(e => e != Element.Ore && e != Element.Fuel)
                    .Select(e => e.Clone())
                    .Append(Element.Ore)
                    .Append(Element.Fuel)
                .ToHashSet()
            };
        }
    }
}
