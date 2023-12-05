namespace _2023
{
    public class Day04
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(13, SolvePart1(ParseInput([
            "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
            "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
            "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
            "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
            "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
            "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"
                ])));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(20117, SolvePart1(ParseInput(File.ReadAllLines("input/day04.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(30, SolvePart2(ParseInput([
            "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
            "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
            "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
            "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
            "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
            "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"
                ])));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(13768818, SolvePart2(ParseInput(File.ReadAllLines("input/day04.txt"))));
        }

        private static int SolvePart1(IEnumerable<(HashSet<int> winningNumbers, int[] ownedNumbers)> input) =>
            input.Select(c => c.ownedNumbers.Count(c.winningNumbers.Contains))
                 .Select(p => p == 0 ? 0 : (1 << p - 1))
                 .Sum();

        private static int SolvePart2(IEnumerable<(HashSet<int> winningNumbers, int[] ownedNumbers)> input)
        {
            var cardWins = input.Select(c => c.ownedNumbers.Count(c.winningNumbers.Contains)).ToArray();
            var copyCount = Enumerable.Range(0, cardWins.Length).Select(_ => 1).ToArray();
            for (var idx = 0; idx < cardWins.Length; idx++)
            {
                if (cardWins[idx] > 0)
                {
                    for (var j = idx + 1; j <= idx + cardWins[idx]; j++)
                    {
                        copyCount[j] += copyCount[idx];
                    }
                }
            }
            return copyCount.Sum();
        }

        private static IEnumerable<(HashSet<int> winningNumbers, int[] ownedNumbers)> ParseInput(IEnumerable<string> input) =>
            input.Select(l => l.Split(':')[1].Trim())
                 .Select(l =>
                 {
                     var numbers = l.Split('|');
                     var winningNumbers = numbers[0].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToHashSet();
                     var ownedNumbers = numbers[1].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();
                     return (winningNumbers, ownedNumbers);
                 });
    }
}
