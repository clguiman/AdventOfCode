namespace Utils
{
    public static class LinqExtensions
    {
        public static IEnumerable<(int x, int y, TSource value)> Where<TSource>(this Grid2D<TSource> source, Func<(int x, int y, TSource value), bool> predicate) => source.Enumerate().Where(predicate);
        public static IEnumerable<TResult> Select<TSource, TResult>(this Grid2D<TSource> source, Func<(int x, int y, TSource value), TResult> selector) => source.Enumerate().Select(selector);
        public static int Count<TSource>(this Grid2D<TSource> source, Func<TSource, bool> predicate) => source.Items.Count(predicate);
    }
}
