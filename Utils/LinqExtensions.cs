namespace Utils
{
    public static class LinqExtensions
    {
        public static IEnumerable<(int x, int y, TSource value)> Where<TSource>(this Grid2D<TSource> source, Func<(int x, int y, TSource value), bool> predicate) => source.Enumerate().Where(predicate);

        public static IEnumerable<TSource> Where<TSource>(this Grid2D<TSource> source, Func<TSource, bool> predicate) => source.Items.Where(predicate);

        public static IEnumerable<TResult> Select<TSource, TResult>(this Grid2D<TSource> source, Func<(int x, int y, TSource value), TResult> selector) => source.Enumerate().Select(selector);

        public static IEnumerable<TResult> Select<TSource, TResult>(this Grid2D<TSource> source, Func<TSource, TResult> selector) => source.Items.Select(selector);

        public static int Count<TSource>(this Grid2DBase<TSource> source, Func<(int x, int y, TSource value), bool> predicate) => source.Enumerate().Count(predicate);

        public static int Count<TSource>(this Grid2DBase<TSource> source, Func<TSource, bool> predicate) => source.Items.Count(predicate);

        public static IEnumerable<int> AsDigits(this string source) => source.Select(x => x - '0');

        public static Grid2D<int> AsDigitGrid(this IEnumerable<string> source) => new(source.Select(line => line.AsDigits()));
    }
}
