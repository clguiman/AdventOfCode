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

        public IEnumerable<T> Enumerate()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    yield return _grid[y][x];
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

        private readonly T[][] _grid;
    }
}