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
            var rowsToDuplicate = new List<int>();
            for (var rowIdx = 0; rowIdx < image.Height; rowIdx++)
            {
                if (image.Rows.Take(new Range(rowIdx, rowIdx + 1)).Single().All(x => x == '.'))
                {
                    rowsToDuplicate.Add(rowIdx);
                }
            }
            var colsToDuplicate = new List<int>();
            for (var colIdx = 0; colIdx < image.Width; colIdx++)
            {
                if (image.Columns.Take(new Range(colIdx, colIdx + 1)).Single().All(x => x == '.'))
                {
                    colsToDuplicate.Add(colIdx);
                }
            }

            var galaxyLocations = image
                .Where(t => t.value == '#')
                .Select(t => new Point2D(
                            t.x + HowManyItemsToAdd(colsToDuplicate, t.x, distanceBetweenEmptySpace),
                            t.y + HowManyItemsToAdd(rowsToDuplicate, t.y, distanceBetweenEmptySpace)))
                .ToArray();

            var distancesToCompute = new List<(Point2D origin, Point2D dest)>();
            for (var idx = 0; idx < galaxyLocations.Length - 1; idx++)
            {
                for (var idx2 = idx + 1; idx2 < galaxyLocations.Length; idx2++)
                {
                    distancesToCompute.Add((galaxyLocations[idx], galaxyLocations[idx2]));
                }
            }

            return distancesToCompute.Select(d => d.origin.ManhattanDistance(d.dest)).Select(x => (long)x).Sum();
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
