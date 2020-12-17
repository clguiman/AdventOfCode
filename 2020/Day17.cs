using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day17
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(112, Part1(
@".#.
..#
###"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(273, Part1(File.ReadAllText("input/day17.txt")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(848, Part2(
@".#.
..#
###"));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(1504, Part2(File.ReadAllText("input/day17.txt")));
        }

        private static long Part1(string input) => Solution(input, false);

        private static long Part2(string input) => Solution(input, true);

        private static long Solution(string input, bool use4Dimensions)
        {
            var pocket = PocketDimension.FromString(input, use4Dimensions ? 4 : 3);
            PocketDimension pocketClone;
            for (var cycle = 0; cycle < 6; cycle++)
            {
                pocketClone = pocket.Clone();
                for (var x = pocketClone.Range.Min; x < pocketClone.Range.Max; x++)
                {
                    for (var y = pocketClone.Range.Min; y < pocketClone.Range.Max; y++)
                    {
                        for (var z = pocketClone.Range.Min; z < pocketClone.Range.Max; z++)
                        {
                            for (var w = use4Dimensions ? pocketClone.Range.Min : 0; w < (use4Dimensions ? pocketClone.Range.Max : 1); w++)
                            {
                                var cur = pocketClone.GetCube(x, y, z, w);
                                if (cur == CubeState.Active)
                                {
                                    var nCount = pocketClone.GetNeighbors(x, y, z, w).Where(c => c == CubeState.Active).Take(4).Count();
                                    if (nCount != 2 && nCount != 3)
                                    {
                                        pocket.SetCube(x, y, z, w, CubeState.Inactive);
                                    }
                                }
                                else if (cur == CubeState.Inactive && pocketClone.GetNeighbors(x, y, z, w).Where(c => c == CubeState.Active).Count() == 3)
                                {
                                    pocket.SetCube(x, y, z, w, CubeState.Active);
                                }
                            }
                        }
                    }
                }
            }

            return pocket.AllStates.Count(s => s == CubeState.Active);
        }

        private enum CubeState
        {
            Active,
            Inactive
        }

        private class PocketDimension
        {
            public static PocketDimension FromString(string input, int dimensions = 3) =>
                new PocketDimension(
                    input.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).First().Trim().Length,
                    dimensions == 4,
                input.Split('\n')
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(x => x.Trim())
                    .Select(line => line.Select(c => c == '#' ? CubeState.Active : CubeState.Inactive))
                    .SelectMany(x => x)
                    .ToArray());

            public CubeState GetCube(int x, int y, int z, int w)
            {
                if (activeCubes.Contains((x, y, z, w)))
                {
                    return CubeState.Active;
                }
                return CubeState.Inactive;
            }

            public void SetCube(int x, int y, int z, int w, CubeState state)
            {
                var item = (x, y, z, w);
                if (state == CubeState.Inactive)
                {
                    if (activeCubes.Contains(item))
                    {
                        activeCubes.Remove(item);
                    }
                }
                else
                {
                    if (!activeCubes.Contains(item))
                    {
                        activeCubes.Add(item);
                        min = item.x < min ? item.x : min;
                        min = item.y < min ? item.y : min;
                        min = item.z < min ? item.z : min;
                        min = item.w < min ? item.w : min;
                        max = item.x > max ? item.x : max;
                        max = item.y > max ? item.y : max;
                        max = item.z > max ? item.z : max;
                        max = item.w > max ? item.w : max;
                    }
                }
            }

            public PocketDimension Clone() => new PocketDimension(size, has4thDimension, activeCubes)
            {
                min = min,
                max = max
            };

            private readonly static IEnumerable<(int x, int y, int z, int w)> NeighborOffsets3D = GenerateNeighborOffsets(false).ToArray();
            private readonly static IEnumerable<(int x, int y, int z, int w)> NeighborOffsets4D = GenerateNeighborOffsets(true).ToArray();

            private static IEnumerable<(int x, int y, int z, int w)> GenerateNeighborOffsets(bool has4D) =>
                Enumerable.Range(-1, 3)
                    .SelectMany(x => Enumerable.Range(-1, 3)
                        .Select(y => (x, y))
                    .SelectMany(item => Enumerable.Range(-1, 3)
                        .Select(z => (item.x, item.y, z))
                    .SelectMany(item => Enumerable.Range(has4D ? -1 : 0, has4D ? 3 : 1)
                        .Select(w => (item.x, item.y, item.z, w)))));

            public IEnumerable<CubeState> GetNeighbors(int x, int y, int z, int w) =>
                (has4thDimension ? NeighborOffsets4D : NeighborOffsets3D)
                    .Select(offset => (x: offset.x + x, y: offset.y + y, z: offset.z + z, w: offset.w + w))
                    .Where(pos => pos.x != x || pos.y != y || pos.z != z || pos.w != w)
                    .Select(pos => activeCubes.Contains(pos) ? CubeState.Active : CubeState.Inactive);

            public IReadOnlyCollection<CubeState> AllStates => activeCubes.Select(x => CubeState.Active).ToArray();

            public (int Min, int Max) Range => (min - 2, max + 2);

            private PocketDimension(int size, bool has4thDimension, CubeState[] cubes)
            {
                this.size = size;
                this.has4thDimension = has4thDimension;
                activeCubes = new();
                for (var y = 0; y < size; y++)
                {
                    for (var x = 0; x < size; x++)
                    {
                        if (cubes[y * size + x] == CubeState.Active)
                        {
                            activeCubes.Add((x, y, 0, 0));
                        }
                    }
                }

                min = 0;
                max = size - 1;
            }

            private PocketDimension(int size, bool has4thDimension, HashSet<(int x, int y, int z, int w)> activeCubes)
            {
                this.size = size;
                this.has4thDimension = has4thDimension;
                this.activeCubes = new(activeCubes);
            }

            private readonly HashSet<(int x, int y, int z, int w)> activeCubes;

            private readonly bool has4thDimension;
            private readonly int size;
            private int min;
            private int max;
        }
    }
}
