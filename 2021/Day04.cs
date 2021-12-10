using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Xunit;

namespace _2021
{
    public class Day04
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(4512, Solve(ParseInput(new[] {
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
            }), true));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(74320, Solve(ParseInput(File.ReadAllLines("input/day04.txt")), true));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(17884, Solve(ParseInput(File.ReadAllLines("input/day04.txt")), false));
        }

        private static int Solve((IEnumerable<Grid2D<(int, bool)>>, IEnumerable<int>) input, bool stopAtFirst)
        {
            Queue<Grid2D<(int, bool)>> boards = new(input.Item1);

            foreach (int number in input.Item2)
            {
                var boardCount = boards.Count;
                for (var idx = 0; idx < boardCount; idx++)
                {
                    var curBoard = boards.Dequeue();
                    //mark as found
                    foreach (var (x, y, value) in curBoard.Where(t => t.value.Item1 == number))
                    {
                        curBoard.SetAt((value.Item1, true), x, y);
                    }

                    bool hasWon = curBoard.Rows.Any(row => row.All(x => x.Item2)) || curBoard.Columns.Any(col => col.All(x => x.Item2));
                    if (!hasWon)
                    {
                        boards.Enqueue(curBoard);
                    }

                    if (hasWon && (stopAtFirst || boards.Count == 0))
                    {
                        return curBoard.Where(x => !x.Item2).Select(x => x.Item1).Sum() * number;
                    }
                }
            }
            return 0;
        }

        private static (IEnumerable<Grid2D<(int, bool)>>, IEnumerable<int>) ParseInput(string[] lines) => (
            lines
                .Skip(2)
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => l.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray())
                .Chunk(5)
                .Select(x => new Grid2D<(int, bool)>(x.Select(l => l.Select(c => (c, false))))),
            lines[0]
                .Split(',')
                .Select(int.Parse)
            );
    }
}
