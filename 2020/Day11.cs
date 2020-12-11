using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day11
    {
        [Fact]
        public void Test1()
        {
            var input =
@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";
            Assert.Equal(37, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(2283, Part1(File.ReadAllText("input/day11.txt")));
        }

        [Fact]
        public void Test3()
        {
            var input =
@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";
            Assert.Equal(26, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(2054, Part2(File.ReadAllText("input/day11.txt")));
        }

        private static int Part1(string input) => RunUntilStable(ParseGrid(input), true, 4)
                .Select(g => g.Count(t => t == TileType.Occupied))
                .Sum();


        private static long Part2(string input) => RunUntilStable(ParseGrid(input), false, 5)
                .Select(g => g.Count(t => t == TileType.Occupied))
                .Sum();

        private static List<List<TileType>> RunUntilStable(List<List<TileType>> grid, bool isVisibleOnlyIfAdjacent, int maxOccupiedSeats)
        {
            var prevGrid = grid;
            var curGrid = CloneGrid(prevGrid);
            var loopCount = 0;
            do
            {
                if (loopCount > 0)
                {
                    prevGrid = curGrid;
                    curGrid = CloneGrid(curGrid);
                }
                curGrid = SingleStep(curGrid,
                    emptyRule: (grid, i, j) => GetVisibleSeats(grid, i, j, isVisibleOnlyIfAdjacent).Count(t => t == TileType.Occupied) == 0,
                    occupiedRule: (grid, i, j) => GetVisibleSeats(grid, i, j, isVisibleOnlyIfAdjacent).Count(t => t == TileType.Occupied) >= maxOccupiedSeats
                    );
                loopCount++;
            } while (!AreGridsIdentical(prevGrid, curGrid));
            return curGrid;
        }

        private static List<List<TileType>> SingleStep(List<List<TileType>> grid, Func<List<List<TileType>>, int, int, bool> emptyRule, Func<List<List<TileType>>, int, int, bool> occupiedRule)
        {
            var ret = CloneGrid(grid);
            for (var i = 0; i < grid.Count; i++)
            {
                for (var j = 0; j < grid[0].Count; j++)
                {
                    if (grid[i][j] == TileType.Empty && emptyRule(grid, i, j))
                    {
                        ret[i][j] = TileType.Occupied;
                    }
                    else if (grid[i][j] == TileType.Occupied && occupiedRule(grid, i, j))
                    {
                        ret[i][j] = TileType.Empty;
                    }
                }
            }
            return ret;
        }

        private static IEnumerable<TileType> GetVisibleSeats(List<List<TileType>> grid, int x, int y, bool isVisibleOnlyIfAdjacent)
        {
            //ugly, need to refactor!
            for (var i = x - 1; i >= 0; i--)
            {
                if (grid[i][y] != TileType.FreeSpace)
                {
                    yield return grid[i][y];
                    break;
                }
                if (isVisibleOnlyIfAdjacent)
                {
                    break;
                }
            }
            var j = y - 1;
            for (var i = x - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (grid[i][j] != TileType.FreeSpace)
                {
                    yield return grid[i][j];
                    break;
                }
                if (isVisibleOnlyIfAdjacent)
                {
                    break;
                }
            }
            j = y + 1;
            for (var i = x - 1; i >= 0 && j <= grid[0].Count - 1; i--, j++)
            {
                if (grid[i][j] != TileType.FreeSpace)
                {
                    yield return grid[i][j];
                    break;
                }
                if (isVisibleOnlyIfAdjacent)
                {
                    break;
                }
            }

            for (var i = x + 1; i <= grid.Count - 1; i++)
            {
                if (grid[i][y] != TileType.FreeSpace)
                {
                    yield return grid[i][y];
                    break;
                }
                if (isVisibleOnlyIfAdjacent)
                {
                    break;
                }
            }

            j = y + 1;
            for (var i = x + 1; i <= grid.Count - 1 && j <= grid[0].Count - 1; i++, j++)
            {
                if (grid[i][j] != TileType.FreeSpace)
                {
                    yield return grid[i][j];
                    break;
                }
                if (isVisibleOnlyIfAdjacent)
                {
                    break;
                }
            }

            j = y - 1;
            for (var i = x + 1; i <= grid.Count - 1 && j >= 0; i++, j--)
            {
                if (grid[i][j] != TileType.FreeSpace)
                {
                    yield return grid[i][j];
                    break;
                }
                if (isVisibleOnlyIfAdjacent)
                {
                    break;
                }
            }

            for (j = y - 1; j >= 0; j--)
            {
                if (grid[x][j] != TileType.FreeSpace)
                {
                    yield return grid[x][j];
                    break;
                }
                if (isVisibleOnlyIfAdjacent)
                {
                    break;
                }
            }

            for (j = y + 1; j < grid[0].Count; j++)
            {
                if (grid[x][j] != TileType.FreeSpace)
                {
                    yield return grid[x][j];
                    break;
                }
                if (isVisibleOnlyIfAdjacent)
                {
                    break;
                }
            }
        }
        private static bool AreGridsIdentical(List<List<TileType>> a, List<List<TileType>> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }
            for (var i = 0; i < a.Count; i++)
            {
                if (a[i].Count != b[i].Count)
                {
                    return false;
                }
                for (var j = 0; j < a[i].Count; j++)
                {
                    if (a[i][j] != b[i][j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static List<List<TileType>> CloneGrid(List<List<TileType>> a)
        {
            var ret = new List<List<TileType>>(a.Count);
            foreach (var x in a)
            {
                ret.Add(x.ToList());
            }
            return ret;
        }

        private static List<List<TileType>> ParseGrid(string input)
        {
            var ret = new List<List<TileType>>();
            foreach (var line in input.Split('\n'))
            {
                if (line.Trim().Length == 0)
                {
                    continue;
                }
                var row = Enumerable
                    .Range(0, line.Length)
                    .Where(x => line[x] == '#' || line[x] == 'L' || line[x] == '.')
                    .Select(x => line[x] == '#' ? TileType.Occupied : line[x] == 'L' ? TileType.Empty : TileType.FreeSpace)
                    .ToList();
                ret.Add(row);
            }
            return ret;
        }

        private enum TileType
        {
            Empty = 1,
            Occupied = 2,
            FreeSpace = 3,
        }
    }
}
