﻿using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2021
{
    public class Day09
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(15, Part1(new[] {
                "2199943210",
                "3987894921",
                "9856789892",
                "8767896789",
                "9899965678"
            }.AsDigitGrid()));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(575, Part1(File.ReadAllLines("input/day09.txt").AsDigitGrid()));
        }


        [Fact]
        public void Test3()
        {
            Assert.Equal(1134, Part2(new[] {
                "2199943210",
                "3987894921",
                "9856789892",
                "8767896789",
                "9899965678"
            }.AsDigitGrid()));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(1019700, Part2(File.ReadAllLines("input/day09.txt").AsDigitGrid()));
        }

        private static int Part1(Grid2D<int> input) => input
            .Where(t => input.GetAdjacentOrthogonalLocations(t.x, t.y).All(adjacent => input.At(adjacent.x, adjacent.y) > t.value))
            .Select(t => t.value + 1)
            .Sum();

        private static int Part2(Grid2D<int> input) => input
            .Where(t => input.GetAdjacentOrthogonalLocations(t.x, t.y).All(adjacent => input.At(adjacent.x, adjacent.y) > t.value))
            .Select(t => (t.x, t.y))
            .Select(lowPoint => input.Clone()
                .BFS(lowPoint,
                    shouldWalkPredicate: (t) => t.possibleAdjacent.item != 9 && t.possibleAdjacent.item > t.current.item,
                    markVisitedFunc: (_) => int.MinValue,
                    useOnlyOrthogonalWalking: true)
                .Count(x => x == int.MinValue))
            .OrderByDescending(x => x)
            .Take(3)
            .Aggregate((a, b) => a * b);
    }
}
