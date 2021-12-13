using System.Text;

namespace Utils
{
    public class Grid2D<T>
    {
        public Grid2D(int width, int height)
        {
           _grid = Enumerable.Range(0, height).Select(_ => new T[width]).ToArray();
            Width = width;
            Height = height;
        }

        public Grid2D(IEnumerable<IEnumerable<T>> input)
        {
            _grid = input.Select(l => l.ToArray()).ToArray();
            Width = _grid[0].Length;
            Height = _grid.Length;
        }

        public int Width { get; init; }

        public int Height { get; init; }

        public IEnumerable<T> Items => Enumerate().Select(t => t.value);

        public IEnumerable<IEnumerable<T>> Rows => EnumerateRows().Select(row => row.Select(x => x.value));

        public IEnumerable<IEnumerable<T>> Columns => EnumerateColumns().Select(col => col.Select(x => x.value));

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

        public IEnumerable<IEnumerable<(int x, int y, T value)>> EnumerateRows()
        {
            for (var y = 0; y < Height; y++)
            {
                yield return Enumerable.Range(0, Width).Select(x => (x, y, _grid[y][x]));
            }
        }

        public IEnumerable<IEnumerable<(int x, int y, T value)>> EnumerateColumns()
        {
            for (var x = 0; x < Width; x++)
            {
                yield return Enumerable.Range(0, Height).Select(y => (x, y, _grid[y][x]));
            }
        }

        public T At(int x, int y) => _grid[y][x];

        public ref T AtRef(int x, int y) => ref _grid[y][x];

        public void SetAt(T val, int x, int y)
        {
            _grid[y][x] = val;
        }

        public Grid2D<T> Clone() => new(_grid);

        public IEnumerable<(int x, int y)> GetAdjacentOrthogonalLocations(int locationX, int locationY)
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

        public IEnumerable<(int x, int y)> GetAllAdjacentLocations(int locationX, int locationY)
        {
            if (locationY > 0)
            {
                yield return (locationX, locationY - 1);

                if (locationX > 0)
                {
                    yield return (locationX - 1, locationY - 1);
                }
                if (locationX < Width - 1)
                {
                    yield return (locationX + 1, locationY - 1);
                }
            }

            if (locationY < Height - 1)
            {
                yield return (locationX, locationY + 1);
                if (locationX > 0)
                {
                    yield return (locationX - 1, locationY + 1);
                }
                if (locationX < Width - 1)
                {
                    yield return (locationX + 1, locationY + 1);
                }
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

        public Grid2D<T> BFS((int x, int y) initialPosition, Predicate<(T currentItem, T possibleAdjacentItem)> shouldWalkPredicate, Func<T, T> markVisitedFunc, bool useOnlyOrthogonalWalking, bool allowReWalk = false)
        {
            List<(int x, int y)> nextSteps = new[] { initialPosition }.ToList();
            while (nextSteps.Count > 0)
            {
                var newPositions = new List<(int x, int y)>();
                foreach (var (curX, curY) in nextSteps)
                {
                    var curItem = At(curX, curY);
                    newPositions.AddRange(
                        (useOnlyOrthogonalWalking ? GetAdjacentOrthogonalLocations(curX, curY) : GetAllAdjacentLocations(curX, curY))
                        .Where(t => shouldWalkPredicate.Invoke((curItem, At(t.x, t.y))))
                        );

                    _grid[curY][curX] = markVisitedFunc(_grid[curY][curX]);
                }
                if (allowReWalk)
                {
                    nextSteps = newPositions.ToList();
                }
                else
                {
                    nextSteps = newPositions.Distinct().ToList();
                }
            }
            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var line in _grid)
            {
                foreach (var item in line)
                {
                    sb.Append(item);
                    sb.Append(' ');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private readonly T[][] _grid;
    }
}