namespace _2023
{
    public class Day09
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(114L, SolvePart1(ParseInput([
            "0 3 6 9 12 15",
            "1 3 6 10 15 21",
            "10 13 16 21 30 45"
                ])));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(1987402313L, SolvePart1(ParseInput(File.ReadAllLines("input/day09.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(2L, SolvePart2(ParseInput([
            "0 3 6 9 12 15",
            "1 3 6 10 15 21",
            "10 13 16 21 30 45"
                ])));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(900L, SolvePart2(ParseInput(File.ReadAllLines("input/day09.txt"))));
        }

        private static long SolvePart1(IEnumerable<long[]> input) => input
            .Select(history => BuildSequenceAndGetEdgeNumbers(history).Sum())
            .Sum();

        private static long SolvePart2(IEnumerable<long[]> input) => input
            .Select(history => BuildSequenceAndGetEdgeNumbers(history, true).Reverse().Aggregate((acc, x) => x - acc))
            .Sum();

        private static IEnumerable<long> BuildSequenceAndGetEdgeNumbers(long[] initialSequence, bool getFirst = false)
        {
            long[] curDiff = initialSequence;
            long[] newDiff = new long[curDiff.Length - 1];
            int sequenceLength = initialSequence.Length;

            while (sequenceLength != 0 && curDiff.Take(sequenceLength).Any(x => x != 0))
            {
                for (var idx = 0; idx < curDiff.Length - 1; idx++)
                {
                    newDiff[idx] = curDiff[idx + 1] - curDiff[idx];
                }
                if (getFirst)
                {
                    yield return curDiff[0];
                }
                else
                {
                    yield return curDiff[sequenceLength - 1];
                }
                (curDiff, newDiff) = (newDiff, curDiff);
                sequenceLength--;
            }
        }

        private static IEnumerable<long[]> ParseInput(IEnumerable<string> input) => input.Select(line => line.Split(' ').Select(long.Parse).ToArray());
    }
}
