namespace Utils
{
    public class Grid2D<T>
    {
        public Grid2D(IEnumerable<IEnumerable<T>> input)
        {
            _grid = input.Select(l => l.ToArray()).ToArray();
            Width = _grid[0].Length;
            Height = _grid.Length;
        }

        public int Width { get; init; }

        public int Height { get; init; }

        public IEnumerable<T> Items => Enumerate().Select(t => t.value);

        public IEnumerable<(int x, int y, T value)> Enumerate()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    yield return (x, y, _grid[y][x]);
                }
            }
        }

        public T At(int x, int y) => _grid[y][x];

        public void SetAt(T val, int x, int y)
        {
            _grid[y][x] = val;
        }

        public Grid2D<T> Clone() => new(_grid);

        public IEnumerable<(int x, int y)> GetAdjacentLocations(int locationX, int locationY)
        {
            if (locationY > 0)
            {
                yield return (locationX, locationY - 1);
            }
            if (locationY < Height - 1)
            {
                yield return (locationX, locationY + 1);
            }
            if (locationX > 0)
            {
                yield return (locationX - 1, locationY);
            }
            if (locationX < Width - 1)
            {
                yield return (locationX + 1, locationY);
            }
        }

        public Grid2D<T> BFS((int x, int y) initialPosition, Predicate<(T currentItem, T possibleAdjacentItem)> shouldWalkPredicate, Func<T, T> markVisitedFunc)
        {
            List<(int x, int y)> nextSteps = new() { initialPosition };
            while (nextSteps.Count > 0)
            {
                var newPositions = new List<(int x, int y)>();
                foreach (var (curX, curY) in nextSteps)
                {
                    var curItem = At(curX, curY);
                    newPositions.AddRange(
                        GetAdjacentLocations(curX, curY)
                        .Where(t => shouldWalkPredicate.Invoke((curItem, At(t.x, t.y))))
                        );

                    _grid[curY][curX] = markVisitedFunc(_grid[curY][curX]);
                }
                nextSteps = newPositions.Distinct().ToList();
            }
            return this;
        }

        private readonly T[][] _grid;
    }
}