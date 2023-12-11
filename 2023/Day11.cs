namespace _2023
{
    public class Day11
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(374, Solve(ParseInput([
            "...#......",
            ".......#..",
            "#.........",
            "..........",
            "......#...",
            ".#........",
            ".........#",
            "..........",
            ".......#..",
            "#...#....."
                ]), 2));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(10494813, Solve(ParseInput(File.ReadAllLines("input/day11.txt")), 2));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(1030, Solve(ParseInput([
            "...#......",
            ".......#..",
            "#.........",
            "..........",
            "......#...",
            ".#........",
            ".........#",
            "..........",
            ".......#..",
            "#...#....."
                ]), 10));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(8410, Solve(ParseInput([
            "...#......",
            ".......#..",
            "#.........",
            "..........",
            "......#...",
            ".#........",
            ".........#",
            "..........",
            ".......#..",
            "#...#....."
                ]), 100));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(840988812853, Solve(ParseInput(File.ReadAllLines("input/day11.txt")), 1_000_000));
        }

        private static long Solve(Grid2D<char> image, int distanceBetweenEmptySpace)
        {
            var rowsToDuplicate = image.Rows
                                        .Select((row, idx) => (row, idx))
                                        .Where(t => t.row.All(x => x == '.'))
                                        .Select(t => t.idx)
                                        .ToList();
            var colsToDuplicate = image.Columns
                                        .Select((col, idx) => (col, idx))
                                        .Where(t => t.col.All(x => x == '.'))
                                        .Select(t => t.idx)
                                        .ToList();

            var galaxyLocations = image
                .Where(t => t.value == '#')
                .Select(t => new Point2D(
                            t.x + HowManyItemsToAdd(colsToDuplicate, t.x, distanceBetweenEmptySpace),
                            t.y + HowManyItemsToAdd(rowsToDuplicate, t.y, distanceBetweenEmptySpace)))
                .ToArray();

            return Enumerable.Range(0, galaxyLocations.Length)
                .Select(idx => Enumerable.Range(idx + 1, galaxyLocations.Length - idx - 1)
                                         .Select(idx2 => (long)galaxyLocations[idx].ManhattanDistance(galaxyLocations[idx2]))
                                         .Sum())
                .Sum();
        }

        private static int HowManyItemsToAdd(List<int> duplicateIndices, int index, int distanceBetweenEmptySpace)
        {
            int spacesToAdd = duplicateIndices.Count;
            for (var idx = 0; idx < duplicateIndices.Count; idx++)
            {
                if (duplicateIndices[idx] > index)
                {
                    spacesToAdd = idx;
                    break;
                }
            }
            return spacesToAdd * distanceBetweenEmptySpace - spacesToAdd;
        }

        private static Grid2D<char> ParseInput(IEnumerable<string> input) => new(input.Select(l => l.Select(x => x).ToArray()));
    }
}
