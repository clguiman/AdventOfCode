namespace _2023
{
    public class Day10
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(4, SolvePart1(ParseInput([
            ".....",
            ".S-7.",
            ".|.|.",
            ".L-J.",
            "....."
                ])));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(8, SolvePart1(ParseInput([
                "..F7.",
                ".FJ|.",
                "SJ.L7",
                "|F--J",
                "LJ...",
                ])));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(6738, SolvePart1(ParseInput(File.ReadAllLines("input/day10.txt"))));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(4, SolvePart2(ParseInput([
            "...........",
            ".S-------7.",
            ".|F-----7|.",
            ".||.....||.",
            ".||.....||.",
            ".|L-7.F-J|.",
            ".|..|.|..|.",
            ".L--J.L--J.",
            "..........."
                ])));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(4, SolvePart2(ParseInput([
            "..........",
            ".S------7.",
            ".|F----7|.",
            ".||....||.",
            ".||....||.",
            ".|L-7F-J|.",
            ".|..||..|.",
            ".L--JL--J.",
            ".........."
                ])));
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal(8, SolvePart2(ParseInput([
            ".F----7F7F7F7F-7....",
            ".|F--7||||||||FJ....",
            ".||.FJ||||||||L7....",
            "FJL7L7LJLJ||LJ.L-7..",
            "L--J.L7...LJS7F-7L7.",
            "....F-J..F7FJ|L7L7L7",
            "....L7.F7||L7|.L7L7|",
            ".....|FJLJ|FJ|F7|.LJ",
            "....FJL-7.||.||||...",
            "....L---J.LJ.LJLJ...",
                ])));
        }

        [Fact]
        public void Test7()
        {
            Assert.Equal(10, SolvePart2(ParseInput([
            "FF7FSF7F7F7F7F7F---7",
            "L|LJ||||||||||||F--J",
            "FL-7LJLJ||||||LJL-77",
            "F--JF--7||LJLJ7F7FJ-",
            "L---JF-JLJ.||-FJLJJ7",
            "|F|F-JF---7F7-L7L|7|",
            "|FFJF7L7F-JF7|JL---7",
            "7-L-JL7||F7|L7F-7F7|",
            "L.L7LFJ|||||FJL7||LJ",
            "L7JLJL-JLJLJL--JLJ.L"
                ])));
        }

        [Fact]
        public void Test8()
        {
            Assert.Equal(579, SolvePart2(ParseInput(File.ReadAllLines("input/day10.txt"))));
        }

        private static int SolvePart1((Grid2D<char> surface, int startX, int startY) input)
        {
            (_, var costMap) = GetLoopAndCostMap(input);

            return costMap.Items.Max();
        }

        private static int SolvePart2((Grid2D<char> surface, int startX, int startY) input)
        {
            (var loopPoints, _) = GetLoopAndCostMap(input);
            var surfaceWithoutLoosePipes = input.surface.Clone() as Grid2D<char>;
            foreach (var (x, y, _) in surfaceWithoutLoosePipes.Enumerate().Where(t => t.value != '.'))
            {
                if (!loopPoints.Contains(new Point2D(x, y)))
                {
                    surfaceWithoutLoosePipes.SetAt('.', x, y);
                }
            }

            (var surface, var insertedRowIndices, var insertedColumnIndices) = AddRoomBetweenPipes(surfaceWithoutLoosePipes);
            Grid2D<bool> visitedMap = new(surface.Width, surface.Height);
            List<Point2D> startPositions = [];

            // color the pipes
            foreach (var (x, y, value) in surface.Where(t => t.value != '.'))
            {
                visitedMap.SetAt(true, x, y);
                startPositions.Add(new Point2D(x, y));
            }

            visitedMap.BFS(new Point2D(0, 0),
                (t) => !t.possibleAdjacent.item && surface.At(t.possibleAdjacent.location) == '.',
                _ => true, useOnlyOrthogonalWalking: true, allowReWalk: false);

            return visitedMap.Where(t => !insertedColumnIndices.Contains(t.x) && !insertedRowIndices.Contains(t.y) && !t.value).Count();
        }

        private static (HashSet<Point2D> loop, Grid2D<int> costMap) GetLoopAndCostMap((Grid2D<char> surface, int startX, int startY) input)
        {
            var surface = input.surface;
            Grid2D<int> costMap = new(surface.Width, surface.Height);

            costMap.BFS(new Point2D(input.startX, input.startY),
                (t) =>
                {
                    if (surface.At(t.possibleAdjacent.location) == '.')
                    {
                        return false;
                    }
                    if (t.possibleAdjacent.item > 0)
                    {
                        return false;
                    }

                    bool shouldWalk = ShouldWalk(
                        surface.At(t.current.location),
                        surface.At(t.possibleAdjacent.location),
                        Grid2DBase<char>.GetDirectionForAdjacentLocation(t.current.location, t.possibleAdjacent.location));

                    if (shouldWalk)
                    {
                        costMap.AtRef(t.possibleAdjacent.location) = costMap.At(t.current.location) + 1;
                    }
                    return shouldWalk;
                },
                _ => _, useOnlyOrthogonalWalking: true, allowReWalk: false);

            return (loop: costMap.Where(t => t.value > 0).Select(t => new Point2D(t.x, t.y)).Append(new Point2D(input.startX, input.startY)).ToHashSet(),
                costMap);
        }

        private static (Grid2D<char> newSurface, HashSet<int> insertedRowIndices, HashSet<int> insertedColumnIndices) AddRoomBetweenPipes(Grid2D<char> surface)
        {
            List<List<char>> newSurface = surface.Rows.Select(x => x.ToList()).Prepend(Enumerable.Range(0, surface.Width + 2).Select(_ => '.').ToList()).ToList();
            HashSet<int> insertedRowIndices = [0];
            HashSet<int> insertedColumnIndices = [0];
            for (var rowIdx = 1; rowIdx < newSurface.Count; rowIdx++)
            {
                newSurface[rowIdx].Add('.');
                newSurface[rowIdx].Insert(0, '.');

                bool shouldInsertRow = false;
                for (var idx = 0; idx < surface.Width; idx++)
                {
                    var top = newSurface[rowIdx - 1][idx];
                    var cur = newSurface[rowIdx][idx];
                    if ((top == '-' && cur == '-') ||
                        (top == '-' && cur == 'F') ||
                        (top == 'J' && cur == '-') ||
                        (top == 'L' && cur == '-') ||
                        (top == 'L' && cur == '7') ||
                        (top == '-' && cur == '7'))
                    {
                        shouldInsertRow = true;
                        break;
                    }
                }

                var newRow = Enumerable.Range(0, surface.Width + 2).Select(_ => '.').ToList();
                if (shouldInsertRow)
                {
                    for (var idx = 0; idx < surface.Width + 2; idx++)
                    {
                        var cur = newSurface[rowIdx][idx];
                        if (cur == '|' || cur == 'J' || cur == 'L')
                        {
                            newRow[idx] = '|';
                        }
                        else
                        {
                            newRow[idx] = '.';
                        }
                    }
                    newSurface.Insert(rowIdx, newRow);
                    insertedRowIndices.Add(rowIdx);
                    rowIdx++;
                }
            }
            newSurface.Add(Enumerable.Range(0, surface.Width + 2).Select(_ => '.').ToList());

            for (var colIdx = 1; colIdx < newSurface[0].Count; colIdx++)
            {
                bool shouldInsertColumn = false;
                for (var idx = 0; idx < newSurface.Count; idx++)
                {
                    var left = newSurface[idx][colIdx - 1];
                    var cur = newSurface[idx][colIdx];
                    if ((left == '|' && cur == '|') ||
                        (left == 'J' && cur == '|') ||
                        (left == '|' && cur == 'F') ||
                        (left == '7' && cur == 'F') ||
                        (left == '7' && cur == 'L') ||
                        (left == 'J' && cur == 'L') ||
                        (left == '|' && cur == 'L'))
                    {
                        shouldInsertColumn = true;
                        break;
                    }
                }

                if (shouldInsertColumn)
                {
                    for (var idx = 0; idx < newSurface.Count; idx++)
                    {
                        var cur = newSurface[idx][colIdx];
                        if (cur == '-' || cur == 'J' || cur == '7')
                        {
                            newSurface[idx].Insert(colIdx, '-');
                        }
                        else
                        {
                            newSurface[idx].Insert(colIdx, '.');
                        }
                    }
                    insertedColumnIndices.Add(colIdx);
                    colIdx++;
                }
            }

            insertedRowIndices.Add(newSurface.Count - 1);
            insertedColumnIndices.Add(newSurface[0].Count - 1);

            return (new Grid2D<char>(newSurface), insertedRowIndices, insertedColumnIndices);
        }

        private static bool ShouldWalk(char curCell, char newCell, Grid2DBase<char>.Direction direction)
        {
            switch (curCell)
            {
                case '|':
                    {
                        if (direction != Grid2DBase<char>.Direction.North && direction != Grid2DBase<char>.Direction.South)
                        {
                            return false;
                        }
                        if (newCell == '|')
                        {
                            return true;
                        }
                        if ((newCell == '7' || newCell == 'F') && direction == Grid2DBase<char>.Direction.North)
                        {
                            return true;
                        }
                        if ((newCell == 'J' || newCell == 'L') && direction == Grid2DBase<char>.Direction.South)
                        {
                            return true;
                        }
                        return false;
                    }
                case '-':
                    {
                        if (direction != Grid2DBase<char>.Direction.East && direction != Grid2DBase<char>.Direction.West)
                        {
                            return false;
                        }
                        if (newCell == '-')
                        {
                            return true;
                        }
                        if ((newCell == 'J' || newCell == '7') && direction == Grid2DBase<char>.Direction.East)
                        {
                            return true;
                        }
                        if ((newCell == 'L' || newCell == 'F') && direction == Grid2DBase<char>.Direction.West)
                        {
                            return true;
                        }
                        return false;
                    }
                case 'L':
                    {
                        if (direction == Grid2DBase<char>.Direction.North)
                        {
                            return newCell == '7' || newCell == 'F' || newCell == '|';
                        }
                        if (direction == Grid2DBase<char>.Direction.East)
                        {
                            return newCell == 'J' || newCell == '7' || newCell == '-';
                        }
                        return false;
                    }
                case 'J':
                    {
                        if (direction == Grid2DBase<char>.Direction.North)
                        {
                            return newCell == '7' || newCell == 'F' || newCell == '|';
                        }
                        if (direction == Grid2DBase<char>.Direction.West)
                        {
                            return newCell == 'L' || newCell == 'F' || newCell == '-';
                        }
                        return false;
                    }
                case '7':
                    {
                        if (direction == Grid2DBase<char>.Direction.South)
                        {
                            return newCell == 'J' || newCell == 'L' || newCell == '|';
                        }
                        if (direction == Grid2DBase<char>.Direction.West)
                        {
                            return newCell == 'L' || newCell == 'F' || newCell == '-';
                        }
                        return false;
                    }
                case 'F':
                    {
                        if (direction == Grid2DBase<char>.Direction.South)
                        {
                            return newCell == 'J' || newCell == 'L' || newCell == '|';
                        }
                        if (direction == Grid2DBase<char>.Direction.East)
                        {
                            return newCell == '7' || newCell == 'J' || newCell == '-';
                        }
                        return false;
                    }
                default: return false;
            }
        }

        private static (Grid2D<char> surface, int startX, int startY) ParseInput(IEnumerable<string> input)
        {
            Grid2D<char> surface = new(input.Select(line => line.Select(c => c)));
            var (startX, startY, value) = surface.Where((t) => t.value == 'S').First();
            // Finding the start position works only with known input.
            if (surface.At(startX, startY + 1) == '|')
            {
                if (surface.At(startX, startY > 0 ? startY - 1 : startY) == '7')
                {
                    surface.AtRef(startX, startY) = '|';
                }
                else if (surface.At(startX + 1, startY) == 'J')
                {
                    surface.AtRef(startX, startY) = 'F';
                }
                else if (surface.At(startX + 1, startY) == '-')
                {
                    surface.AtRef(startX, startY) = 'F';
                }
                else if (surface.At(startX - 1, startY) == 'F')
                {
                    surface.AtRef(startX, startY) = '7';
                }
            }
            else if (surface.At(startX, startY + 1) == 'J')
            {
                if (surface.At(startX + 1, startY) == '7')
                {
                    surface.AtRef(startX, startY) = 'F';
                }
            }
            return (surface, startX, startY);
        }
    }
}
