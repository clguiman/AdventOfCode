using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2021
{
    public class Day19
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(79, Part1(ParseInput(new[] {
"--- scanner 0 ---","404,-588,-901","528,-643,409","-838,591,734","390,-675,-793","-537,-823,-458","-485,-357,347","-345,-311,381","-661,-816,-575",
"-876,649,763","-618,-824,-621","553,345,-567","474,580,667","-447,-329,318","-584,868,-557","544,-627,-890","564,392,-477","455,729,728","-892,524,684",
"-689,845,-530","423,-701,434","7,-33,-71","630,319,-379","443,580,662","-789,900,-551","459,-707,401","","--- scanner 1 ---","686,422,578","605,423,415",
"515,917,-361","-336,658,858","95,138,22","-476,619,847","-340,-569,-846","567,-361,727","-460,603,-452","669,-402,600","729,430,532","-500,-761,534",
"-322,571,750","-466,-666,-811","-429,-592,574","-355,545,-477","703,-491,-529","-328,-685,520","413,935,-424","-391,539,-444","586,-435,557","-364,-763,-893",
"807,-499,-711","755,-354,-619","553,889,-390","","--- scanner 2 ---","649,640,665","682,-795,504","-784,533,-524","-644,584,-595","-588,-843,648","-30,6,44",
"-674,560,763","500,723,-460","609,671,-379","-555,-800,653","-675,-892,-343","697,-426,-610","578,704,681","493,664,-388","-671,-858,530",
"-667,343,800","571,-461,-707","-138,-166,112","-889,563,-600","646,-828,498","640,759,510","-630,509,768","-681,-892,-333","673,-379,-804","-742,-814,-386",
"577,-820,562","","--- scanner 3 ---","-589,542,597","605,-692,669","-500,565,-823","-660,373,557","-458,-679,-417","-488,449,543","-626,468,-788","338,-750,-386",
"528,-832,-391","562,-778,733","-938,-730,414","543,643,-506","-524,371,-870","407,773,750","-104,29,83","378,-903,-323","-778,-728,485","426,699,580",
"-438,-605,-362","-469,-447,-387","509,732,623","647,635,-688","-868,-804,481","614,-800,639","595,780,-596","","--- scanner 4 ---","727,592,562","-293,-554,779",
"441,611,-461","-714,465,-776","-743,427,-804","-660,-479,-426","832,-632,460","927,-485,-438","408,393,-506","466,436,-512","110,16,151","-258,-428,682",
"-393,719,612","-211,-452,876","808,-476,-593","-575,615,604","-485,667,467","-680,325,-822","-627,-443,-432","872,-547,-609","833,512,582","807,604,487",
"839,-516,451","891,-625,532","-652,-548,-490","30,-46,-14",
            })));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(383, Part1(ParseInput(File.ReadAllLines("input/day19.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(3621, Part2(ParseInput(new[] {
"--- scanner 0 ---","404,-588,-901","528,-643,409","-838,591,734","390,-675,-793","-537,-823,-458","-485,-357,347","-345,-311,381","-661,-816,-575",
"-876,649,763","-618,-824,-621","553,345,-567","474,580,667","-447,-329,318","-584,868,-557","544,-627,-890","564,392,-477","455,729,728","-892,524,684",
"-689,845,-530","423,-701,434","7,-33,-71","630,319,-379","443,580,662","-789,900,-551","459,-707,401","","--- scanner 1 ---","686,422,578","605,423,415",
"515,917,-361","-336,658,858","95,138,22","-476,619,847","-340,-569,-846","567,-361,727","-460,603,-452","669,-402,600","729,430,532","-500,-761,534",
"-322,571,750","-466,-666,-811","-429,-592,574","-355,545,-477","703,-491,-529","-328,-685,520","413,935,-424","-391,539,-444","586,-435,557","-364,-763,-893",
"807,-499,-711","755,-354,-619","553,889,-390","","--- scanner 2 ---","649,640,665","682,-795,504","-784,533,-524","-644,584,-595","-588,-843,648","-30,6,44",
"-674,560,763","500,723,-460","609,671,-379","-555,-800,653","-675,-892,-343","697,-426,-610","578,704,681","493,664,-388","-671,-858,530",
"-667,343,800","571,-461,-707","-138,-166,112","-889,563,-600","646,-828,498","640,759,510","-630,509,768","-681,-892,-333","673,-379,-804","-742,-814,-386",
"577,-820,562","","--- scanner 3 ---","-589,542,597","605,-692,669","-500,565,-823","-660,373,557","-458,-679,-417","-488,449,543","-626,468,-788","338,-750,-386",
"528,-832,-391","562,-778,733","-938,-730,414","543,643,-506","-524,371,-870","407,773,750","-104,29,83","378,-903,-323","-778,-728,485","426,699,580",
"-438,-605,-362","-469,-447,-387","509,732,623","647,635,-688","-868,-804,481","614,-800,639","595,780,-596","","--- scanner 4 ---","727,592,562","-293,-554,779",
"441,611,-461","-714,465,-776","-743,427,-804","-660,-479,-426","832,-632,460","927,-485,-438","408,393,-506","466,436,-512","110,16,151","-258,-428,682",
"-393,719,612","-211,-452,876","808,-476,-593","-575,615,604","-485,667,467","-680,325,-822","-627,-443,-432","872,-547,-609","833,512,582","807,604,487",
"839,-516,451","891,-625,532","-652,-548,-490","30,-46,-14",
            })));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(9854, Part2(ParseInput(File.ReadAllLines("input/day19.txt"))));
        }

        private static long Part1(List<List<Vec3>> scanners)
        {
            return Solve(scanners).points.Count;
        }

        private static long Part2(List<List<Vec3>> scanners)
        {
            (_, var scannerPositions) = Solve(scanners);

            long max = 0;
            for (var i = 0; i < scannerPositions.Count - 1; i++)
            {
                for (var j = i + 1; j < scannerPositions.Count; j++)
                {
                    var d = scannerPositions[i].ManhattanDistance(scannerPositions[j]);
                    if (d > max)
                    {
                        max = d;
                    }
                }
            }
            return max;
        }

        private static (List<Vec3> points, List<Vec3> scannerPositions) Solve(List<List<Vec3>> scanners)
        {
            var commonReference = scanners[0].ToList();
            var leftoverScanners = new Queue<List<Vec3>>(scanners.Skip(1));
            var scannerPositions = new List<Vec3>() { Vec3.Zero };
            while (leftoverScanners.Count > 0)
            {
                var curScanner = leftoverScanners.Dequeue();
                (var res, var offset) = ResolveScanner2RelativeToScanner1(commonReference, curScanner);
                if (res.Count == 0)
                {
                    leftoverScanners.Enqueue(curScanner);
                    continue;
                }
                commonReference = res;
                scannerPositions.Add(offset);
            }
            return (commonReference, scannerPositions);
        }

        private static (List<Vec3> resolvedPoints, Vec3 offset) ResolveScanner2RelativeToScanner1(List<Vec3> scanner1, List<Vec3> scanner2)
        {
            List<Vec3> ret = new();
            var uniqueDistances = new Dictionary<long, (int x, int y)>[2];
            var scannerIdx = 0;
            foreach (var beacons in new[] { scanner1, scanner2 })
            {
                Dictionary<long, List<(int x, int y)>> distanceHistogram = new();
                for (var i = 0; i < beacons.Count - 1; i++)
                {
                    for (var j = i + 1; j < beacons.Count; j++)
                    {
                        var d = beacons[i].DistanceSqr(beacons[j]);
                        if (distanceHistogram.ContainsKey(d))
                        {
                            distanceHistogram[d].Add((i, j));
                        }
                        else
                        {
                            distanceHistogram[d] = new() { (i, j) };
                        }
                    }
                }
                uniqueDistances[scannerIdx++] = distanceHistogram.Where(t => t.Value.Count == 1).ToDictionary(t => t.Key, t => t.Value[0]);
            }
            HashSet<(int s1Idx1, int s1Idx2, int s2Idx1, int s2Idx2)> commonDistances = new();
            foreach (var distancesInScanner1 in uniqueDistances[0])
            {
                if (uniqueDistances[1].TryGetValue(distancesInScanner1.Key, out var matchingDistanceInScanner2))
                {
                    commonDistances.Add((distancesInScanner1.Value.x, distancesInScanner1.Value.y, matchingDistanceInScanner2.x, matchingDistanceInScanner2.y));
                }
            }

            if (commonDistances.Count < 2)
            {
                //Not enough common distances to compute
                return (new List<Vec3>(), Vec3.Zero);
            }
            var commonPoints = GetNormalizedCommonPoints(commonDistances, scanner1, scanner2);
            if (commonPoints.Length == 0)
            {
                //Not enough common distances to compute
                return (new List<Vec3>(), Vec3.Zero);
            }

            var pointsInScanner1 = new[] { commonPoints[0].Scanner1Point1, commonPoints[0].Scanner1Point2 };
            var pointsInScanner2 = new[] { commonPoints[0].Scanner2Point1, commonPoints[0].Scanner2Point2 };

            Grid2D<int> rotationTransform = null;
            Vec3 offsetTransform = null;
            foreach (var possibleRotation in Vec3.UnitRotations)
            {
                Vec3 firstPossibleOffset = pointsInScanner1[0].Substract(pointsInScanner2[0].Rotate(possibleRotation));
                Vec3 secondPossibleOffset = pointsInScanner1[1].Substract(pointsInScanner2[1].Rotate(possibleRotation));
                if (firstPossibleOffset.Equals(secondPossibleOffset))
                {
                    // found rotation and offset
                    rotationTransform = possibleRotation;
                    offsetTransform = firstPossibleOffset;
                    break;
                }
            }

            if (rotationTransform == null)
            {
                return (new List<Vec3>(), Vec3.Zero);
            }

            HashSet<Vec3> beconsInScanner1Reference = scanner1.ToHashSet();
            foreach (var beacon in scanner2)
            {
                beconsInScanner1Reference.Add(beacon.Rotate(rotationTransform).Add(offsetTransform));
            }
            return (beconsInScanner1Reference.ToList(), offsetTransform);
        }

        private static (Vec3 Scanner1Point1, Vec3 Scanner1Point2, Vec3 Scanner2Point1, Vec3 Scanner2Point2)[]
            GetNormalizedCommonPoints(
            HashSet<(int s1Idx1, int s1Idx2, int s2Idx1, int s2Idx2)> commonDistances,
            List<Vec3> scanner1,
            List<Vec3> scanner2)
        {
            (int s1Idx1, int s1Idx2, int s2Idx1, int s2Idx2) ref1;
            (int s1Idx1, int s1Idx2, int s2Idx1, int s2Idx2)[] commonPoints;
            var skp = 0;
            do
            {
                ref1 = commonDistances.Skip(skp).First();
                commonPoints = commonDistances.Where(t => t.s1Idx1 == ref1.s1Idx1)
                    .Where(t => t.s1Idx1 != ref1.s1Idx1 || t.s1Idx2 != ref1.s1Idx2 || t.s2Idx1 != ref1.s2Idx1 || t.s2Idx2 != ref1.s2Idx2)
                    .ToArray();
                skp++;
                if (skp >= commonDistances.Count() && commonPoints.Length == 0)
                {
                    return Array.Empty<(Vec3 Scanner1Point1, Vec3 Scanner1Point2, Vec3 Scanner2Point1, Vec3 Scanner2Point2)>();
                }
            } while (commonPoints.Length == 0);
            var ref2 = commonPoints[0];

            // make sure ref2 s2Idx1 is the common index in s2
            if (ref1.s2Idx1 != ref2.s2Idx1)
            {
                if (ref1.s2Idx2 == ref2.s2Idx2)
                {
                    ref2 = (ref2.s1Idx1, ref2.s1Idx2, ref2.s2Idx2, ref2.s2Idx1);
                }
                else
                {
                    if (ref1.s2Idx1 == ref2.s2Idx2)
                    {
                        ref2 = (ref2.s1Idx1, ref2.s1Idx2, ref2.s2Idx2, ref2.s2Idx1);
                    }
                    else if (ref1.s2Idx2 == ref2.s2Idx1)
                    {
                        // s2Idx1 is already the common index
                    }
                    else
                    {
                        throw new Exception("There's no common index!");
                    }
                }
            }

            // normalize common points
            return commonPoints
                .Select(t => (t.s1Idx1, t.s1Idx2, t.s2Idx1 == ref2.s2Idx1 ? t.s2Idx1 : t.s2Idx2, t.s2Idx1 == ref2.s2Idx1 ? t.s2Idx2 : t.s2Idx1))
                .Select(t => (scanner1[t.s1Idx1], scanner1[t.s1Idx2], scanner2[t.Item3], scanner2[t.Item4]))
                .ToArray();
        }

        private static List<List<Vec3>> ParseInput(string[] input)
        {
            var scannerId = 0;
            List<List<Vec3>> beaconPositions = Enumerable.Range(0, 50).Select(_ => new List<Vec3>()).ToList();
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (line.StartsWith("--- scanner"))
                {
                    scannerId = int.Parse(line.Substring(12).Split(' ')[0]);
                    continue;
                }
                var s = line.Split(',').Select(int.Parse).ToArray();
                beaconPositions[scannerId].Add(new(s[0], s[1], s[2]));

            }

            beaconPositions.RemoveRange(scannerId + 1, beaconPositions.Count - scannerId - 1);
            return beaconPositions;
        }

        private class Vec3
        {
            public Vec3(int x, int y, int z) : this(x, y, z, true)
            {
                Basis = new(X >= 0 ? 1 : -1, Y >= 0 ? 1 : -1, Z >= 0 ? 1 : -1, true);
            }

            private Vec3(int x, int y, int z, bool _)
            {
                X = x;
                Y = y;
                Z = z;
                Basis = this;
            }

            public int X { get; }
            public int Y { get; }
            public int Z { get; }

            public Vec3 Basis { get; }

            public long DistanceSqr(Vec3 rhs) => (X - rhs.X) * (X - rhs.X) + (Y - rhs.Y) * (Y - rhs.Y) + (Z - rhs.Z) * (Z - rhs.Z);

            public long ManhattanDistance(Vec3 rhs) => Math.Abs(X - rhs.X) + Math.Abs(Y - rhs.Y) + Math.Abs(Z - rhs.Z);

            public Vec3 Rotate(Grid2D<int> rot) => new(
                X * rot.At(0, 0) + Y * rot.At(1, 0) + Z * rot.At(2, 0),
                X * rot.At(0, 1) + Y * rot.At(1, 1) + Z * rot.At(2, 1),
                X * rot.At(0, 2) + Y * rot.At(1, 2) + Z * rot.At(2, 2));

            public int Dot(Vec3 rhs) => X * rhs.X + Y * rhs.Y + Z * rhs.Z;

            public Vec3 CrossProduct(Vec3 rhs) => new(Y * rhs.Z - Z * rhs.Y, Z * rhs.X - X * rhs.Z, X * rhs.Y - Y * rhs.X);

            public Vec3 Add(Vec3 rhs) => new(X + rhs.X, Y + rhs.Y, Z + rhs.Z);

            public Vec3 Substract(Vec3 rhs) => new(X - rhs.X, Y - rhs.Y, Z - rhs.Z);

            public Vec3 WithX(int x) => new(x, Y, Z);

            public Vec3 WithY(int y) => new(X, y, Z);

            public Vec3 WithZ(int z) => new(X, Y, z);

            public override bool Equals(object obj)
            {
                if (obj is Vec3)
                {
                    var p = obj as Vec3;
                    return X == p.X && Y == p.Y && Z == p.Z;
                }
                return false;
            }

            public override int GetHashCode() => HashCode.Combine(X, Y, Z);

            public override string ToString() => $"{{{X},{Y},{Z}}}";

            public static Vec3 Zero => new(0, 0, 0);

            private static Grid2D<int>[] ComputeUnitRotations()
            {
                // https://en.wikipedia.org/wiki/Rotation_matrix#General_rotations
                var angles = Enumerable.Range(-1, 3)
                                    .SelectMany(x => Enumerable.Range(-1, 3).Select(y => (sin: x, cos: y)))
                                    .Where(t => t.sin != t.cos && t.sin != -1 * t.cos)
                                    .ToArray();
                var t2 = angles.SelectMany(yaw => angles.SelectMany(pitch => angles.Select(roll => (yaw, pitch, roll)))).ToArray();

                return t2.Select(t =>
                {
                    var rotationMatrix = new Grid2D<int>(3, 3);
                    rotationMatrix.SetAt(t.yaw.cos * t.pitch.cos, 0, 0);
                    rotationMatrix.SetAt(t.yaw.cos * t.pitch.sin * t.roll.sin - t.yaw.sin * t.roll.cos, 1, 0);
                    rotationMatrix.SetAt(t.yaw.cos * t.pitch.sin * t.roll.cos + t.yaw.sin * t.roll.sin, 2, 0);
                    rotationMatrix.SetAt(t.yaw.sin * t.pitch.cos, 0, 1);
                    rotationMatrix.SetAt(t.yaw.sin * t.pitch.sin * t.roll.sin + t.yaw.cos * t.roll.cos, 1, 1);
                    rotationMatrix.SetAt(t.yaw.sin * t.pitch.sin * t.roll.cos - t.yaw.cos * t.roll.sin, 2, 1);
                    rotationMatrix.SetAt(-1 * t.pitch.sin, 0, 2);
                    rotationMatrix.SetAt(t.pitch.cos * t.roll.sin, 1, 2);
                    rotationMatrix.SetAt(t.pitch.cos * t.roll.cos, 2, 2);
                    return rotationMatrix;
                }).ToArray();
            }

            public static readonly Grid2D<int>[] UnitRotations = ComputeUnitRotations();
        }
    }
}

