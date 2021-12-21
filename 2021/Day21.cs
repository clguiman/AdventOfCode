using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day21
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(739785, Part1(4, 8));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(678468, Part1(7, 2));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(444356092776315, Part2(4, 8));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(131180774190079, Part2(7, 2));
        }

        private static long Part1(int p1, int p2)
        {
            Player player1 = new(p1);
            Player player2 = new(p2);
            var die = new DeterministicDie(100);
            for (var idx = 1; ; idx++)
            {
                var moves = die.Roll3();
                if (idx % 2 == 1)
                {
                    player1 = player1.Move(moves);
                }
                else
                {
                    player2 = player2.Move(moves);
                }

                if (player1.Score >= 1000)
                {
                    return player2.Score * idx * 3;
                }
                if (player2.Score >= 1000)
                {
                    return player1.Score * idx * 3;
                }
            }
        }

        private static long Part2(int p1, int p2)
        {
            long u1 = 0;
            long u2 = 0;

            var player1 = RunStep(new Dictionary<Player, long>() { { new(p1), 1 } });
            var player2 = new Dictionary<Player, long>() { { new(p2), 1 } };

            while (player1.Count > 0 || player2.Count > 0)
            {
                var state1 = RunStep(player1);
                var state2 = RunStep(player2);

                var noWin1 = state1.Where(x => x.Key.Score < 21).Select(x => x.Value).Sum();
                var noWin2 = state2.Where(x => x.Key.Score < 21).Select(x => x.Value).Sum();

                u1 += RemoveWinningStatesAndCountUniverses(state1, noWin2);
                u2 += RemoveWinningStatesAndCountUniverses(state2, noWin1);
                player1 = state1;
                player2 = state2;

            }

            if (u1 > u2)
            {
                return u1;
            }
            return u2;
        }

        private static Dictionary<Player, long> RunStep(Dictionary<Player, long> previousState)
        {
            var nextState = new Dictionary<Player, long>();
            foreach (var possibleRoll in PossibleDiracRolls)
            {
                foreach (var prevOutcome in previousState)
                {
                    var newOutcome = prevOutcome.Key.Move(possibleRoll);
                    if (nextState.ContainsKey(newOutcome))
                    {
                        nextState[newOutcome] += prevOutcome.Value;
                    }
                    else
                    {
                        nextState.Add(newOutcome, prevOutcome.Value);
                    }
                }
            }
            return nextState;
        }

        private static long RemoveWinningStatesAndCountUniverses(Dictionary<Player, long> state, long otherNonWiningCount)
        {
            long ret = 0;
            foreach (var winningUnivers in state.Where(x => x.Key.Score >= 21).ToArray())
            {
                ret += winningUnivers.Value * otherNonWiningCount;
                state.Remove(winningUnivers.Key);
            }
            return ret;
        }

        private static readonly int[] PossibleDiracRolls = Enumerable.Range(1, 3)
                                                        .SelectMany(x => Enumerable.Range(1, 3)
                                                                        .SelectMany(y => Enumerable.Range(1, 3)
                                                                                    .Select(z => (x, y, z))))
                                                        .Select(t => t.x + t.y + t.z).ToArray();

        private class Player
        {
            public Player(int initialPosition) : this(initialPosition, 0)
            {
            }

            public int Position { get; }
            public int Score { get; }
            public int RollCount { get; }

            public Player Move(int steps)
            {
                var pos = Position + steps;
                if (pos > 10)
                {
                    pos %= 10;
                    if (pos == 0)
                    {
                        pos = 10;
                    }
                }
                return new(pos, Score + pos);
            }

            public override bool Equals(object obj)
            {
                if (obj is Player p)
                {
                    return p.Position == Position && p.Score == Score;
                }
                return false;
            }

            public override int GetHashCode() => HashCode.Combine(Position, Score);

            public override string ToString() => $"{Position} - {Score}";

            private Player(int initialPosition, int score)
            {
                Position = initialPosition;
                Score = score;
            }


        }

        private class DeterministicDie
        {
            public DeterministicDie(int sides)
            {
                _sides = sides;
                _nextRoll = 1;
            }

            public int Roll3()
            {
                if (_nextRoll == _sides - 2)
                {
                    _nextRoll = 1;
                    return (_sides - 1) * 3;
                }
                else if (_nextRoll == _sides - 1)
                {
                    _nextRoll = 2;
                    return 2 * _sides;
                }
                else if (_nextRoll == _sides)
                {
                    _nextRoll = 3;
                    return _sides + 3;
                }
                else
                {
                    _nextRoll += 3;
                    return (_nextRoll - 2) * 3;
                }
            }

            private readonly int _sides;
            private int _nextRoll;
        }
    }
}

