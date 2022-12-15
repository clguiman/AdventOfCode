using System.Text;

namespace Utils
{
    public class Grid2D<T> : Grid2DBase<T>
    {
        public Grid2D(int width, int height) : base()
        {
            _grid = Enumerable.Range(0, height).Select(_ => new T[width]).ToArray();
            Width = width;
            Height = height;
        }

        public Grid2D(IEnumerable<IEnumerable<T>> input) : base()
        {
            _grid = input.Select(l => l.ToArray()).ToArray();
            Width = _grid[0].Length;
            Height = _grid.Length;
        }

        public int Width { get; init; }

        public int Height { get; init; }

        public IEnumerable<IEnumerable<T>> Rows => EnumerateRows().Select(row => row.Select(x => x.value));

        public IEnumerable<IEnumerable<T>> Columns => EnumerateColumns().Select(col => col.Select(x => x.value));

        public override IEnumerable<(int x, int y, T value)> Enumerate()
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

        public override T At(int x, int y) => _grid[y][x];

        public ref T AtRef(int x, int y) => ref _grid[y][x];

        public override void SetAt(T val, int x, int y)
        {
            _grid[y][x] = val;
        }

        public override T At(Point2D location) => At(location.X, location.Y);

        public ref T AtRef(Point2D location) => ref AtRef(location.X, location.Y);

        public override void SetAt(T val, Point2D location) => SetAt(val, location.X, location.Y);

        public override Grid2DBase<T> Clone() => new Grid2D<T>(_grid);
        protected override bool IsCoordinateValid(Point2D coordinate) =>
            coordinate.X >= 0 && coordinate.X < Width && coordinate.Y >= 0 && coordinate.Y < Height;

        protected override (int Width, int Height) GetGridDimensions() => (Width, Height);

        public override string ToString()
        {
            return ToString(' ');
        }

        public string ToString(char? separator)
        {
            var sb = new StringBuilder();
            foreach (var line in _grid)
            {
                foreach (var item in line)
                {
                    sb.Append(item);
                    if (separator != null)
                    {
                        sb.Append(separator);
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private readonly T[][] _grid;
    }
}