namespace Utils
{
    public class SparseGrid2D<T> : Grid2DBase<T>
    {
        public SparseGrid2D(T defaultValue) : base()
        {
            _defaultValue = defaultValue;
        }

        public override IEnumerable<(int x, int y, T value)> Enumerate() => _values.Select(t => (x: t.Key.X, y: t.Key.Y, value: t.Value));

        public override T At(Point2D location)
        {
            if (_values.TryGetValue(location, out var ret))
            {
                return ret;
            }
            return _defaultValue;
        }

        public override void SetAt(T val, Point2D location)
        {
            _values[location] = val;
        }

        public override T At(int x, int y) => At(new(x, y));

        public override void SetAt(T val, int x, int y) => SetAt(val, new(x, y));

        public override Grid2DBase<T> Clone()
        {
            var ret = new SparseGrid2D<T>(_defaultValue);
            foreach (var item in _values)
            {
                ret._values.Add(item.Key, item.Value);
            }
            return ret;
        }

        protected override bool IsCoordinateValid(Point2D coordinate) => true;

        protected override (int Width, int Height) GetGridDimensions()
        {
            if (_values.Count == 0)
            {
                return (1, 1);
            }
            var firstVal = _values.Keys.First();

            var minX = firstVal.X;
            var maxX = firstVal.X;
            var minY = firstVal.Y;
            var maxY = firstVal.Y;
            foreach (var k in _values.Keys)
            {
                if (k.X > maxX)
                {
                    maxX = k.X;
                }
                if (k.X < minX)
                {
                    minX = k.X;
                }
                if (k.Y > maxY)
                {
                    maxY = k.Y;
                }
                if (k.Y < minY)
                {
                    minY = k.Y;
                }
            }
            return (maxX - minX + 1, maxY - minY + 1);
        }

        private readonly Dictionary<Point2D, T> _values = new();
        private readonly T _defaultValue;
    }
}
