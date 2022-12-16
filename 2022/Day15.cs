using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2022
{
    public class Day15
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(26, Part1(ParseInput(new[] {
                "Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
                "Sensor at x=9, y=16: closest beacon is at x=10, y=16",
                "Sensor at x=13, y=2: closest beacon is at x=15, y=3",
                "Sensor at x=12, y=14: closest beacon is at x=10, y=16",
                "Sensor at x=10, y=20: closest beacon is at x=10, y=16",
                "Sensor at x=14, y=17: closest beacon is at x=10, y=16",
                "Sensor at x=8, y=7: closest beacon is at x=2, y=10",
                "Sensor at x=2, y=0: closest beacon is at x=2, y=10",
                "Sensor at x=0, y=11: closest beacon is at x=2, y=10",
                "Sensor at x=20, y=14: closest beacon is at x=25, y=17",
                "Sensor at x=17, y=20: closest beacon is at x=21, y=22",
                "Sensor at x=16, y=7: closest beacon is at x=15, y=3",
                "Sensor at x=14, y=3: closest beacon is at x=15, y=3",
                "Sensor at x=20, y=1: closest beacon is at x=15, y=3"
            }), 10));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(5511201, Part1(ParseInput(File.ReadAllLines("input/day15.txt")), 2_000_000));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(56_000_011, Part2(ParseInput(new[] {
                "Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
                "Sensor at x=9, y=16: closest beacon is at x=10, y=16",
                "Sensor at x=13, y=2: closest beacon is at x=15, y=3",
                "Sensor at x=12, y=14: closest beacon is at x=10, y=16",
                "Sensor at x=10, y=20: closest beacon is at x=10, y=16",
                "Sensor at x=14, y=17: closest beacon is at x=10, y=16",
                "Sensor at x=8, y=7: closest beacon is at x=2, y=10",
                "Sensor at x=2, y=0: closest beacon is at x=2, y=10",
                "Sensor at x=0, y=11: closest beacon is at x=2, y=10",
                "Sensor at x=20, y=14: closest beacon is at x=25, y=17",
                "Sensor at x=17, y=20: closest beacon is at x=21, y=22",
                "Sensor at x=16, y=7: closest beacon is at x=15, y=3",
                "Sensor at x=14, y=3: closest beacon is at x=15, y=3",
                "Sensor at x=20, y=1: closest beacon is at x=15, y=3"
            }), 20));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(11_318_723_411_840, Part2(ParseInput(File.ReadAllLines("input/day15.txt")), 4_000_000));
        }


        private static long Part1(((int x, int y) sensor, (int x, int y) beacon, int distance)[] data, int row)
        {
            var leftmost = data.MinBy(t => t.beacon.x);
            var rightmost = data.MaxBy(t => t.beacon.x);

            var left = leftmost.beacon.x - Math.Abs(leftmost.distance - row);
            var right = rightmost.beacon.x + Math.Abs(rightmost.distance - row);

            return Enumerable.Range(left, right - left + 1).Where(idx =>
                data.Any(d =>
                    (row != d.sensor.y || idx != d.sensor.x) &&
                    (row != d.beacon.y || idx != d.beacon.x) &&
                    ComputeDistance((idx, row), d.sensor) <= d.distance
                    )
                ).Count();
        }

        private static long Part2(((int x, int y) sensor, (int x, int y) beacon, int distance)[] data, int maxCoordinates)
        {
            var coordinates = GetBorderCoordinates(data).Where(t => t.x >= 0 && t.x <= maxCoordinates && t.y >= 0 && t.y <= maxCoordinates);

            foreach ((var x, var y) in coordinates)
            {
                if (!data.Any(d => (y == d.sensor.y && x == d.sensor.x) ||
                                   (y == d.beacon.y && x == d.beacon.x) ||
                                   (ComputeDistance((x, y), d.sensor) <= d.distance)))
                {
                    return (long)x * 4_000_000 + (long)y;
                }
            }
            return 0;
        }

        private static IEnumerable<(int x, int y)> GetBorderCoordinates(IEnumerable<((int x, int y) sensor, (int x, int y) beacon, int distance)> data)
        {
            foreach (var d in data)
            {
                var x = d.sensor.x + d.distance + 1;
                var y = d.sensor.y;
                for (var idx = 0; idx < d.distance; idx++)
                {
                    yield return (x, y);
                    x--;
                    y++;
                }
                for (var idx = 0; idx < d.distance; idx++)
                {
                    yield return (x, y);
                    x++;
                    y--;
                }
                for (var idx = 0; idx < d.distance; idx++)
                {
                    yield return (x, y);
                    x--;
                    y--;
                }
                for (var idx = 0; idx < d.distance; idx++)
                {
                    yield return (x, y);
                    x++;
                    y++;
                }
            }
        }

        private static ((int x, int y) sensor, (int x, int y) beacon, int distance)[] ParseInput(IEnumerable<string> input) => ParseInput2(input).ToArray();

        private static IEnumerable<((int x, int y) sensor, (int x, int y) beacon, int distance)> ParseInput2(IEnumerable<string> input)
        {
            int sensorStartIdx = "Sensor at x=".Length;
            int beaconStartIdx = " closest beacon is at x=".Length;
            foreach (var line in input)
            {
                var tokens = line.Split(':');
                var sensorCoordinateTokens = tokens[0][sensorStartIdx..].Split(',');
                var sensorX = int.Parse(sensorCoordinateTokens[0]);
                var sensorY = int.Parse(sensorCoordinateTokens[1][3..]);

                var beaconCoordinateTokens = tokens[1][beaconStartIdx..].Split(',');
                var beaconX = int.Parse(beaconCoordinateTokens[0]);
                var beaconY = int.Parse(beaconCoordinateTokens[1][3..]);

                yield return ((sensorX, sensorY), (beaconX, beaconY), distance: ComputeDistance((sensorX, sensorY), (beaconX, beaconY)));
            }
        }

        private static int ComputeDistance((int x, int y) sensor, (int x, int y) beacon)
        {
            return Math.Abs(beacon.x - sensor.x) + Math.Abs(beacon.y - sensor.y);
        }
    }
}