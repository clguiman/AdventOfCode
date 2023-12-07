using System.Text;

namespace _2023
{
    public class Day07
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(6440, Solve(ParseInput([
            "32T3K 765",
            "T55J5 684",
            "KK677 28",
            "KTJJT 220",
            "QQQJA 483"
                ])));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(252656917, Solve(ParseInput(File.ReadAllLines("input/day07.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(5905, Solve(ParseInput([
            "32T3K 765",
            "T55J5 684",
            "KK677 28",
            "KTJJT 220",
            "QQQJA 483"
                ], hasJokers: true)));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(253499763, Solve(ParseInput(File.ReadAllLines("input/day07.txt"), hasJokers: true)));
        }
        private static int Solve(IEnumerable<(Hand hand, int bid)> input) => input
            .OrderBy(x => x.hand)
            .Select((x, idx) => x.bid * (idx + 1))
            .Sum();

        private static IEnumerable<(Hand hand, int bid)> ParseInput(IEnumerable<string> input, bool hasJokers = false) => input
            .Select(line =>
            {
                var r = line.Split(' ');
                return (new Hand(r[0], hasJokers), int.Parse(r[1]));
            });

        private record Hand : IComparable<Hand>
        {
            public Hand(string hand, bool hasJokers)
            {
                if (hand.Length != 5)
                {
                    throw new ArgumentException(nameof(hand));
                }
                Cards = hand.Select(c => c switch
                    {
                        '2' => Card.Two,
                        '3' => Card.Three,
                        '4' => Card.Four,
                        '5' => Card.Five,
                        '6' => Card.Six,
                        '7' => Card.Seven,
                        '8' => Card.Eight,
                        '9' => Card.Nine,
                        'T' => Card.Ten,
                        'J' => hasJokers ? Card.Joker : Card.Jack,
                        'Q' => Card.Queen,
                        'K' => Card.King,
                        'A' => Card.Ace,
                        _ => throw new ArgumentException(nameof(hand)),
                    }
                ).ToArray();

                var distinctCount = Cards.Distinct().Count();
                var jokerCount = hasJokers ? Cards.Count(x => x == Card.Joker) : 0;
                if (distinctCount == 1)
                {
                    Type = HandType.FiveOfAKind;
                }
                else if (distinctCount == 2)
                {
                    if (jokerCount > 0)
                    {
                        Type = HandType.FiveOfAKind;
                    }
                    else
                    {
                        if (Cards.GroupBy(x => x).Count(x => x.Count() == 4) == 1)
                        {
                            Type = HandType.FourOfAKind;
                        }
                        else
                        {
                            Type = HandType.FullHouse;
                        }
                    }
                }
                else if (distinctCount == 3)
                {
                    if (Cards.GroupBy(x => x).Count(x => x.Count() == 2) == 2)
                    {
                        if (jokerCount == 1)
                        {
                            Type = HandType.FullHouse;
                        }
                        else if (jokerCount == 2)
                        {
                            Type = HandType.FourOfAKind;
                        }
                        else
                        {
                            Type = HandType.TwoPairs;
                        }
                    }
                    else
                    {
                        if (jokerCount > 0)
                        {
                            Type = HandType.FourOfAKind;
                        }
                        else
                        {
                            Type = HandType.ThreeOfAKind;
                        }
                    }
                }
                else if (distinctCount == 4)
                {
                    if (jokerCount > 0)
                    {
                        Type = HandType.ThreeOfAKind;
                    }
                    else
                    {
                        Type = HandType.OnePair;
                    }
                }
                else if (distinctCount == 5)
                {
                    if (jokerCount > 0)
                    {
                        Type = HandType.OnePair;
                    }
                    else
                    {
                        Type = HandType.HighCard;
                    }
                }
                else
                {
                    throw new ArgumentException(nameof(hand));
                }
            }

            public readonly Card[] Cards;

            public readonly HandType Type;

            public int CompareTo(Hand other)
            {
                if (Type < other.Type)
                {
                    return -1;
                }
                if (Type > other.Type)
                {
                    return 1;
                }
                for (var idx = 0; idx < 5; idx++)
                {
                    if (Cards[idx] < other.Cards[idx])
                    {
                        return -1;
                    }
                    if (Cards[idx] > other.Cards[idx])
                    {
                        return 1;
                    }
                }
                return 0;
            }

            public override string ToString()
            {
                StringBuilder sb = new();
                foreach (var c in Cards)
                {
                    sb.Append(c switch
                    {
                        Card.Two => '2',
                        Card.Three => '3',
                        Card.Four => '4',
                        Card.Five => '5',
                        Card.Six => '6',
                        Card.Seven => '7',
                        Card.Eight => '8',
                        Card.Nine => '9',
                        Card.Ten => 'T',
                        Card.Joker => 'J',
                        Card.Jack => 'J',
                        Card.King => 'K',
                        Card.Queen => 'Q',
                        Card.Ace => 'A',
                        _ => throw new InvalidDataException()
                    });
                }
                return sb.ToString();
            }
        }

        private enum Card
        {
            Joker = 0,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 12,
            Queen = 13,
            King = 14,
            Ace = 15
        }

        private enum HandType
        {
            HighCard = 1,
            OnePair = 2,
            TwoPairs = 3,
            ThreeOfAKind = 4,
            FullHouse = 5,
            FourOfAKind = 6,
            FiveOfAKind = 7
        }
    }
}
