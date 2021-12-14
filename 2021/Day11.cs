using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2021
{
    public class Day11
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(1656, Part1(new[] {
                "5483143223",
                "2745854711",
                "5264556173",
                "6141336146",
                "6357385478",
                "4167524645",
                "2176841721",
                "6882881134",
                "4846848554",
                "5283751526"
            }.AsDigitGrid()));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(1675, Part1(File.ReadAllLines("input/day11.txt").AsDigitGrid()));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(195, Part2(new[] {
                "5483143223",
                "2745854711",
                "5264556173",
                "6141336146",
                "6357385478",
                "4167524645",
                "2176841721",
                "6882881134",
                "4846848554",
                "5283751526"
            }.AsDigitGrid()));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(515, Part2(File.ReadAllLines("input/day11.txt").AsDigitGrid()));
        }

        private static int Part1(Grid2D<int> octopuses) => Enumerable.Range(0, 100)
            .Select(_ => RunStep(octopuses)
                .Where(x => x == 0)
                .Count())
            .Sum();

        private static int Part2(Grid2D<int> octopuses)
        {
            var step = 0;
            for (; !octopuses.Items.All(x => x == 0); RunStep(octopuses), step++)
            {
            }

            return step;
        }

        private static Grid2D<int> RunStep(Grid2D<int> octopuses)
        {
            List<(int x, int y)> flashingPositions = new();
            foreach (var (x, y, value) in octopuses.Enumerate())
            {
                if (value <= 8)
                {
                    octopuses.SetAt(value + 1, x, y);
                    continue;
                }
                flashingPositions.Add((x, y));
            }

            if (flashingPositions.Count > 0)
            {
                foreach (var flashingPos in flashingPositions)
                {
                    octopuses.BFS(flashingPos,
                            shouldWalkPredicate: t => (t.currentItem == 9) && t.possibleAdjacentItem >= 1 && t.possibleAdjacentItem <= 9,
                            markVisitedFunc: (x) => (x == 0 || x == 9) ? 0 : x + 1,
                            onNextLevel: (_) => { },
                            useOnlyOrthogonalWalking: false,
                            allowReWalk: true);
                }
            }

            return octopuses;
        }
    }
}
