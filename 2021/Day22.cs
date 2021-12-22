using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2021
{
    public class Day22
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(39, Part1(ParseInput(new[] {
                "on x=10..12,y=10..12,z=10..12",
                "on x=11..13,y=11..13,z=11..13",
                "off x=9..11,y=9..11,z=9..11",
                "on x=10..10,y=10..10,z=10..10"
            })));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(590784, Part1(ParseInput(new[] {
                "on x=-20..26,y=-36..17,z=-47..7",
                "on x=-20..33,y=-21..23,z=-26..28",
                "on x=-22..28,y=-29..23,z=-38..16",
                "on x=-46..7,y=-6..46,z=-50..-1",
                "on x=-49..1,y=-3..46,z=-24..28",
                "on x=2..47,y=-22..22,z=-23..27",
                "on x=-27..23,y=-28..26,z=-21..29",
                "on x=-39..5,y=-6..47,z=-3..44",
                "on x=-30..21,y=-8..43,z=-13..34",
                "on x=-22..26,y=-27..20,z=-29..19",
                "off x=-48..-32,y=26..41,z=-47..-37",
                "on x=-12..35,y=6..50,z=-50..-2",
                "off x=-48..-32,y=-32..-16,z=-15..-5",
                "on x=-18..26,y=-33..15,z=-7..46",
                "off x=-40..-22,y=-38..-28,z=23..41",
                "on x=-16..35,y=-41..10,z=-47..6",
                "off x=-32..-23,y=11..30,z=-14..3",
                "on x=-49..-5,y=-3..45,z=-29..18",
                "off x=18..30,y=-20..-8,z=-3..13",
                "on x=-41..9,y=-7..43,z=-33..15",
                "on x=-54112..-39298,y=-85059..-49293,z=-27449..7877",
                "on x=967..23432,y=45373..81175,z=27513..53682"
            })));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(474140, Part1(ParseInput(new[] {
                "on x=-5..47,y=-31..22,z=-19..33",
                "on x=-44..5,y=-27..21,z=-14..35",
                "on x=-49..-1,y=-11..42,z=-10..38",
                "on x=-20..34,y=-40..6,z=-44..1",
                "off x=26..39,y=40..50,z=-2..11",
                "on x=-41..5,y=-41..6,z=-36..8",
                "off x=-43..-33,y=-45..-28,z=7..25",
                "on x=-33..15,y=-32..19,z=-34..11",
                "off x=35..47,y=-46..-34,z=-11..5",
                "on x=-14..36,y=-6..44,z=-16..29"
            })));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(590467, Part1(ParseInput(File.ReadAllLines("input/day22.txt"))));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(2758514936282235, Solve(ParseInput(new[] {
                "on x=-5..47,y=-31..22,z=-19..33",
                "on x=-44..5,y=-27..21,z=-14..35",
                "on x=-49..-1,y=-11..42,z=-10..38",
                "on x=-20..34,y=-40..6,z=-44..1",
                "off x=26..39,y=40..50,z=-2..11",
                "on x=-41..5,y=-41..6,z=-36..8",
                "off x=-43..-33,y=-45..-28,z=7..25",
                "on x=-33..15,y=-32..19,z=-34..11",
                "off x=35..47,y=-46..-34,z=-11..5",
                "on x=-14..36,y=-6..44,z=-16..29",
                "on x=-57795..-6158,y=29564..72030,z=20435..90618",
                "on x=36731..105352,y=-21140..28532,z=16094..90401",
                "on x=30999..107136,y=-53464..15513,z=8553..71215",
                "on x=13528..83982,y=-99403..-27377,z=-24141..23996",
                "on x=-72682..-12347,y=18159..111354,z=7391..80950",
                "on x=-1060..80757,y=-65301..-20884,z=-103788..-16709",
                "on x=-83015..-9461,y=-72160..-8347,z=-81239..-26856",
                "on x=-52752..22273,y=-49450..9096,z=54442..119054",
                "on x=-29982..40483,y=-108474..-28371,z=-24328..38471",
                "on x=-4958..62750,y=40422..118853,z=-7672..65583",
                "on x=55694..108686,y=-43367..46958,z=-26781..48729",
                "on x=-98497..-18186,y=-63569..3412,z=1232..88485",
                "on x=-726..56291,y=-62629..13224,z=18033..85226",
                "on x=-110886..-34664,y=-81338..-8658,z=8914..63723",
                "on x=-55829..24974,y=-16897..54165,z=-121762..-28058",
                "on x=-65152..-11147,y=22489..91432,z=-58782..1780",
                "on x=-120100..-32970,y=-46592..27473,z=-11695..61039",
                "on x=-18631..37533,y=-124565..-50804,z=-35667..28308",
                "on x=-57817..18248,y=49321..117703,z=5745..55881",
                "on x=14781..98692,y=-1341..70827,z=15753..70151",
                "on x=-34419..55919,y=-19626..40991,z=39015..114138",
                "on x=-60785..11593,y=-56135..2999,z=-95368..-26915",
                "on x=-32178..58085,y=17647..101866,z=-91405..-8878",
                "on x=-53655..12091,y=50097..105568,z=-75335..-4862",
                "on x=-111166..-40997,y=-71714..2688,z=5609..50954",
                "on x=-16602..70118,y=-98693..-44401,z=5197..76897",
                "on x=16383..101554,y=4615..83635,z=-44907..18747",
                "off x=-95822..-15171,y=-19987..48940,z=10804..104439",
                "on x=-89813..-14614,y=16069..88491,z=-3297..45228",
                "on x=41075..99376,y=-20427..49978,z=-52012..13762",
                "on x=-21330..50085,y=-17944..62733,z=-112280..-30197",
                "on x=-16478..35915,y=36008..118594,z=-7885..47086",
                "off x=-98156..-27851,y=-49952..43171,z=-99005..-8456",
                "off x=2032..69770,y=-71013..4824,z=7471..94418",
                "on x=43670..120875,y=-42068..12382,z=-24787..38892",
                "off x=37514..111226,y=-45862..25743,z=-16714..54663",
                "off x=25699..97951,y=-30668..59918,z=-15349..69697",
                "off x=-44271..17935,y=-9516..60759,z=49131..112598",
                "on x=-61695..-5813,y=40978..94975,z=8655..80240",
                "off x=-101086..-9439,y=-7088..67543,z=33935..83858",
                "off x=18020..114017,y=-48931..32606,z=21474..89843",
                "off x=-77139..10506,y=-89994..-18797,z=-80..59318",
                "off x=8476..79288,y=-75520..11602,z=-96624..-24783",
                "on x=-47488..-1262,y=24338..100707,z=16292..72967",
                "off x=-84341..13987,y=2429..92914,z=-90671..-1318",
                "off x=-37810..49457,y=-71013..-7894,z=-105357..-13188",
                "off x=-27365..46395,y=31009..98017,z=15428..76570",
                "off x=-70369..-16548,y=22648..78696,z=-1892..86821",
                "on x=-53470..21291,y=-120233..-33476,z=-44150..38147",
                "off x=-93533..-4276,y=-16170..68771,z=-104985..-24507"
            })));
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal(1225064738333321, Solve(ParseInput(File.ReadAllLines("input/day22.txt"))));
        }

        private static long Part1(IEnumerable<CuboidAction> input)
        {
            return Solve(input.Where(step => Math.Abs(step.Cuboid.X1) <= 50 &&
                                             Math.Abs(step.Cuboid.X2) <= 50 &&
                                             Math.Abs(step.Cuboid.Y1) <= 50 &&
                                             Math.Abs(step.Cuboid.Y2) <= 50 &&
                                             Math.Abs(step.Cuboid.Z1) <= 50 &&
                                             Math.Abs(step.Cuboid.Z2) <= 50));
        }

        private static long Solve(IEnumerable<CuboidAction> input)
        {
            List<Cuboid> lightedCubes = new() { input.First().Cuboid };
            foreach (var step in input.Skip(1))
            {
                if (step.TurnOn)
                {
                    lightedCubes.Add(step.Cuboid);
                }
                else
                {
                    List<Cuboid> newLights = new();
                    foreach (var light in lightedCubes)
                    {
                        var intersection = step.Cuboid.Intersection(light);
                        if (intersection.Equals(Cuboid.Zero))
                        {
                            newLights.Add(light);
                            continue;
                        }
                        newLights.AddRange(light.SplitInNonOverlapingCuboids(intersection));
                    }
                    lightedCubes = newLights;
                }
            }
            lightedCubes = ReduceOverlappingCuboids(lightedCubes);
            return lightedCubes.Select(c => c.Volume).Sum();
        }

        private static List<Cuboid> ReduceOverlappingCuboidsInternal(List<Cuboid> regions)
        {
            List<Cuboid> ret = regions.ToList();
            for (var intersectionWasFound = true; intersectionWasFound;)
            {
                intersectionWasFound = false;
                HashSet<Cuboid> leftover = new();
                for (var i = 0; i < ret.Count; i++)
                {
                    bool noIntersectionsFound = true;
                    for (var j = i + 1; j < ret.Count; j++)
                    {
                        var intersection = ret[i].Intersection(ret[j]);
                        if (intersection.Equals(Cuboid.Zero))
                        {
                            continue;
                        }
                        intersectionWasFound = true;
                        noIntersectionsFound = false;

                        leftover.Add(intersection);
                        foreach (var nl in ret[i].SplitInNonOverlapingCuboids(ret[j]))
                        {
                            leftover.Add(nl);
                        }

                        for (var k = i + 1; k < j; k++)
                        {
                            if (k != j)
                            {
                                leftover.Add(ret[k]);
                            }
                        }
                        i = j;
                        break;
                    }
                    if (noIntersectionsFound)
                    {
                        leftover.Add(ret[i]);
                    }
                }
                ret = leftover.ToList();
            }
            return ret;
        }

        private static List<Cuboid> ReduceOverlappingCuboids(List<Cuboid> regions, int maxDepth = 25)
        {
            if (regions.Count < 10 || maxDepth <= 0)
            {
                return ReduceOverlappingCuboidsInternal(regions);
            }

            var reducedCuboids = regions;
            for (var step = 0; step < 4; step++)
            {
                reducedCuboids = (maxDepth % 6) switch
                {
                    0 => reducedCuboids.OrderBy(c => c.X1).ToList(),
                    1 => reducedCuboids.OrderBy(c => c.X2).ToList(),
                    2 => reducedCuboids.OrderBy(c => c.Y1).ToList(),
                    3 => reducedCuboids.OrderBy(c => c.Y2).ToList(),
                    4 => reducedCuboids.OrderBy(c => c.Z1).ToList(),
                    _ => reducedCuboids.OrderBy(c => c.Z2).ToList(),
                };

                var partitionCount = 2 * Environment.ProcessorCount;
                var partitionSize = (reducedCuboids.Count / partitionCount) + 1;

                var partitionedResults = Enumerable.Range(0, partitionCount)
                                            .Select(idx => reducedCuboids.Skip(idx * partitionSize).Take(partitionSize).ToList())
                                            .AsParallel()
                                            .Select(ReduceOverlappingCuboidsInternal)
                                            .ToArray();
                reducedCuboids.Clear();
                foreach (var p in partitionedResults)
                {
                    reducedCuboids.AddRange(p);
                }
            }

            return ReduceOverlappingCuboids(reducedCuboids, maxDepth - 1);
        }

        private static IEnumerable<CuboidAction> ParseInput(IEnumerable<string> input)
        {
            foreach (var line in input)
            {
                bool turnOn = false;
                if (line[1] == 'n')
                {
                    turnOn = true;
                }
                var coordinates = line.Split("x=")[1].Split("..")
                        .SelectMany(x => x.Split(','))
                        .Select(c => (c.Length > 2 && c[1] == '=') ? c[2..] : c)
                        .Select(int.Parse)
                        .ToArray();

                yield return new(turnOn, coordinates[0], coordinates[1], coordinates[2], coordinates[3], coordinates[4], coordinates[5]);
            }
        }

        private class Cuboid
        {
            public Cuboid(int x1, int x2, int y1, int y2, int z1, int z2)
            {
                X1 = x1;
                X2 = x2;
                Y1 = y1;
                Y2 = y2;
                Z1 = z1;
                Z2 = z2;
            }
            public long Volume => ((long)X2 - (long)X1 + 1) * ((long)Y2 - (long)Y1 + 1) * ((long)Z2 - (long)Z1 + 1);
            public int X1 { get; }
            public int X2 { get; }
            public int Y1 { get; }
            public int Y2 { get; }
            public int Z1 { get; }
            public int Z2 { get; }

            public IEnumerable<Cuboid> SplitInNonOverlapingCuboids(Cuboid other)
            {
                var intersection = Intersection(other);
                if (intersection.Equals(Zero))
                {
                    return new[] { this, other };
                }
                return RemoveInnerCuboid(intersection).Concat(other.RemoveInnerCuboid(intersection));
            }

            private IEnumerable<Cuboid> RemoveInnerCuboid(Cuboid inner)
            {
                if (inner.Equals(Zero))
                {
                    yield return this;
                }
                if (this.Z1 < inner.Z1)
                {
                    yield return new(X1, X2, Y1, Y2, Z1, inner.Z1 - 1);
                }
                if (this.Z2 > inner.Z2)
                {
                    yield return new(X1, X2, Y1, Y2, inner.Z2 + 1, Z2);
                }
                if (this.X1 < inner.X1)
                {
                    yield return new(X1, inner.X1 - 1, Y1, Y2, inner.Z1, inner.Z2);
                }
                if (this.X2 > inner.X2)
                {
                    yield return new(inner.X2 + 1, X2, Y1, Y2, inner.Z1, inner.Z2);
                }
                if (this.Y1 < inner.Y1)
                {
                    yield return new(inner.X1, inner.X2, Y1, inner.Y1 - 1, inner.Z1, inner.Z2);
                }
                if (this.Y2 > inner.Y2)
                {
                    yield return new(inner.X1, inner.X2, inner.Y2 + 1, Y2, inner.Z1, inner.Z2);
                }
            }

            private static (int r1, int r2) ComputeOverlap1D(int x1, int x2, int y1, int y2)
            {
                if (y1 >= x1 && y1 <= x2)
                {
                    return (y1, Math.Min(x2, y2));
                }
                if (y2 >= x1 && y2 <= x2)
                {
                    return (x1, y2);
                }
                if (x1 >= y1 && x1 <= y2 && x2 >= y1 && x2 <= y2)
                {
                    return (x1, x2);
                }
                return (int.MinValue, int.MinValue);
            }

            public Cuboid Intersection(Cuboid other)
            {
                var x = ComputeOverlap1D(other.X1, other.X2, this.X1, this.X2);
                if (x.r1 == int.MinValue || x.r2 == int.MinValue)
                {
                    return Zero;
                }

                var y = ComputeOverlap1D(other.Y1, other.Y2, this.Y1, this.Y2);
                if (y.r1 == int.MinValue || y.r2 == int.MinValue)
                {
                    return Zero;
                }
                var z = ComputeOverlap1D(other.Z1, other.Z2, this.Z1, this.Z2);
                if (z.r1 == int.MinValue || z.r2 == int.MinValue)
                {
                    return Zero;
                }
                return new(x.r1, x.r2, y.r1, y.r2, z.r1, z.r2);
            }

            public override string ToString() => $"x={X1}..{X2},y={Y1}..{Y2},z={Z1}..{Z2}";

            public override bool Equals(object obj)
            {
                if (obj is Cuboid c)
                {
                    return X1 == c.X1 && X2 == c.X2 && Y1 == c.Y1 && Y2 == c.Y2 && Z1 == c.Z1 && Z2 == c.Z2;
                }
                return false;
            }
            public override int GetHashCode() => HashCode.Combine(X1, X2, Y1, Y2, Z1, Z2);

            public static Cuboid Zero = new(0, -1, 0, -1, 0, -1);
        }

        private class CuboidAction
        {
            public CuboidAction(bool turnOn, int x1, int x2, int y1, int y2, int z1, int z2)
            {
                TurnOn = turnOn;
                Cuboid = new(x1, x2, y1, y2, z1, z2);
            }

            public bool TurnOn { get; }
            public Cuboid Cuboid { get; }

            public override string ToString() => $"{ (TurnOn ? "on" : "off")} {Cuboid}";
        }
    }
}

