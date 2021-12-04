using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day04
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(4512, Part1(ParseInput(new[] {
                "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1",
                "",
                "22 13 17 11  0",
                " 8  2 23  4 24",
                "21  9 14 16  7",
                " 6 10  3 18  5",
                " 1 12 20 15 19",
                "",
                " 3 15  0  2 22",
                " 9 18 13 17  5",
                "19  8  7 25 23",
                "20 11 10 24  4",
                "14 21 16 12  6",
                "",
                "14 21 17 24  4",
                "10 16 15  9 19",
                "18  8 23 26 20",
                "22 11 13  6  5",
                " 2  0 12  3  7"
            })));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(74320, Part1(ParseInput(File.ReadAllLines("input/day04.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(17884, Part2(ParseInput(File.ReadAllLines("input/day04.txt"))));
        }

        private static int Part1((Board[], int[]) input)
        {
            var boards = input.Item1;
            foreach (int number in input.Item2)
            {
                foreach (var board in boards)
                {
                    board.MarkAsFound(number);
                    if (board.HasWon)
                    {
                        return board.AllSquares().Where(x => !x.Item2).Select(x => x.Item1).Sum() * number;
                    }
                }
            }
            return 0;
        }

        private static int Part2((Board[], int[]) input)
        {
            var boards = input.Item1;
            foreach (int number in input.Item2)
            {
                foreach (var board in boards)
                {
                    board.MarkAsFound(number);
                }
                if (boards.Length == 1)
                {
                    if (boards[0].HasWon)
                    {
                        return boards[0].AllSquares().Where(x => !x.Item2).Select(x => x.Item1).Sum() * number;
                    }
                }
                else
                {
                    boards = boards.Where(b => !b.HasWon).ToArray();
                }
            }
            return 0;
        }

        private static (Board[] boards, int[] numbers) ParseInput(string[] lines)
        {
            var numbers = lines[0].Split(',').Select(int.Parse).ToArray();
            var boards = lines
                .Skip(2)
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => l.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray())
                .Chunk(5)
                .Select(x => new Board(x, 5)).ToArray();

            return (boards, numbers);
        }

        private class Board
        {
            public Board(IEnumerable<int[]> input, int size)
            {
                _map = input.Select(line => line.Select(x => (x, false)).ToArray()).ToArray();
                _size = size;
            }

            public void MarkAsFound(int x)
            {
                for (var i = 0; i < _size; i++)
                {
                    for (var j = 0; j < _size; j++)
                    {
                        if (_map[i][j].Item1 == x)
                        {
                            _map[i][j].Item2 = true;
                            return;
                        }
                    }
                }
            }

            public IEnumerable<(int, bool)> AllSquares()
            {
                for (var i = 0; i < _size; i++)
                {
                    for (var j = 0; j < _size; j++)
                    {
                        yield return _map[i][j];

                    }
                }
            }

            public bool HasWon
            {
                get
                {
                    foreach (var line in _map)
                    {
                        if (line.Count(x => x.Item2 == true) == _size)
                        {
                            return true;
                        }
                    }

                    for (var col = 0; col < _size; col++)
                    {
                        if (_map.Count(x => x[col].Item2 == true) == _size)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            private (int, bool)[][] _map;
            private int _size;
        }



    }
}
