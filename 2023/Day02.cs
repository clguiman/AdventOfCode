namespace _2023
{
    public class Day02
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(8, SolvePart1(ParseInput([
                "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
                "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
                "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
                "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
                "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",
                ])));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(1734, SolvePart1(ParseInput(File.ReadAllLines("input/day02.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(2286, SolvePart2(ParseInput([
                "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
                "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
                "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
                "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
                "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",
                ])));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(70387, SolvePart2(ParseInput(File.ReadAllLines("input/day02.txt"))));
        }

        private static int SolvePart1(IEnumerable<Game> games) => games
            .Where(g => g.Sets.All(s =>
                            s.Cubes.Where(c => c.Color == Color.Blue).Sum(c => c.Count) <= 14 &&
                            s.Cubes.Where(c => c.Color == Color.Red).Sum(c => c.Count) <= 12 &&
                            s.Cubes.Where(c => c.Color == Color.Green).Sum(c => c.Count) <= 13))
            .Sum(g => g.Id);

        private static int SolvePart2(IEnumerable<Game> games) => games
            .Select(g =>
            {
                var max = g.Sets
                           .Select(s => (
                                red: s.Cubes.Where(c => c.Color == Color.Red).Sum(c => c.Count),
                                green: s.Cubes.Where(c => c.Color == Color.Green).Sum(c => c.Count),
                                blue: s.Cubes.Where(c => c.Color == Color.Blue).Sum(c => c.Count)))
                           .Aggregate((a, b) => (
                              red: Math.Max(a.red, b.red),
                              green: Math.Max(a.green, b.green),
                              blue: Math.Max(a.blue, b.blue)));
                return max.red * max.green * max.blue;
            })
            .Sum();

        private static IEnumerable<Game> ParseInput(IEnumerable<string> input) =>
            input
            .Select(l =>
                {
                    var t = l.Split(':');
                    return (int.Parse(t[0].Split(' ')[1]), t[1]);
                })
            .Select(g => new Game(g.Item1,
                                    g.Item2.Split(';')
                                            .Select(set => set.Split(',')
                                                            .Select(x => x.Trim())
                                                            .Select(x =>
                                                            {
                                                                var t = x.Split(' ');
                                                                var n = int.Parse(t[0]);
                                                                var c = t[1] switch
                                                                {
                                                                    "red" => Color.Red,
                                                                    "green" => Color.Green,
                                                                    "blue" => Color.Blue,
                                                                    _ => throw new InvalidDataException()
                                                                };
                                                                return new Cubes(n, c);
                                                            })
                                                            .ToArray())
                                            .Select(g => new GameSet([.. g]))
                                            .ToArray()));

        private enum Color
        {
            Red,
            Green,
            Blue
        }
        private record Cubes(int Count, Color Color);

        private record GameSet(Cubes[] Cubes);

        private record Game(int Id, GameSet[] Sets);
    }
}
