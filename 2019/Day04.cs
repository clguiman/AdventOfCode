using System;
using Xunit;

namespace _2019
{
    public class Day04
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(2, Solution1(new[] { 1, 1, 1, 1, 1, 1 }, new[] { 1, 1, 1, 1, 1, 2 }));
            Assert.Equal(1890, Solution1(new[] { 1, 3, 8, 2, 4, 1 }, new[] { 6, 7, 4, 0, 3, 4 }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(1, Solution2(new[] { 1, 1, 2, 2, 3, 3 }, new[] { 1, 1, 2, 2, 3, 3 }));
            Assert.Equal(0, Solution2(new[] { 1, 2, 3, 4, 4, 4 }, new[] { 1, 2, 3, 4, 4, 4 }));
            Assert.Equal(1, Solution2(new[] { 1, 1, 1, 1, 2, 2 }, new[] { 1, 1, 1, 1, 2, 2 }));
            Assert.Equal(0, Solution2(new[] { 1, 1, 1, 2, 2, 2 }, new[] { 1, 1, 1, 2, 2, 2 }));
            Assert.Equal(1, Solution2(new[] { 1, 1, 2, 2, 3, 4 }, new[] { 1, 1, 2, 2, 3, 4 }));
            Assert.Equal(1, Solution2(new[] { 1, 2, 2, 3, 3, 3 }, new[] { 1, 2, 2, 3, 3, 3 }));
            Assert.Equal(1277, Solution2(new[] { 1, 3, 8, 2, 4, 1 }, new[] { 6, 7, 4, 0, 3, 4 }));
        }

        private static int Solution1(int[] low, int[] high)
        {
            // brute force
            var cur = new int[6];
            var total = 0;
            low.CopyTo(cur, 0);
            while (LessOrEqualThan(cur, high))
            {
                if (IsMatch(cur))
                {
                    total++;
                }
                Increment(cur);
            }
            return total;
        }

        private static int Solution2(int[] low, int[] high)
        {
            // brute force
            var cur = new int[6];
            var total = 0;
            low.CopyTo(cur, 0);
            while (LessOrEqualThan(cur, high))
            {
                if (IsMatch2(cur))
                {
                    total++;
                }
                Increment(cur);
            }
            return total;
        }

        private static bool LessOrEqualThan(ReadOnlySpan<int> a, ReadOnlySpan<int> b)
        {
            for (var i = 0; i < 6; i++)
            {
                if (a[i] > b[i])
                {
                    return false;
                }
                else if (a[i] < b[i])
                {
                    return true;
                }
            }
            return true;
        }

        private static bool IsMatch(ReadOnlySpan<int> a)
        {
            var result = false;
            var prevDigit = a[0];
            for (var i = 1; i < 6; i++)
            {
                if (a[i] < prevDigit)
                {
                    return false;
                }
                else if (a[i] == prevDigit)
                {
                    result = true;
                }
                prevDigit = a[i];
            }
            return result;
        }

        private static bool IsMatch2(ReadOnlySpan<int> a)
        {
            var result = false;
            var prevDigit = a[0];
            for (var i = 1; i < 6; i++)
            {
                if (a[i] < prevDigit)
                {
                    return false;
                }
                else if (!result && a[i] == prevDigit)
                {
                    result = ((i == 1) || (a[i] != a[i - 2])) && ((i == 5) || (a[i] != a[i + 1]));
                }
                prevDigit = a[i];
            }
            return result;
        }

        private static void Increment(Span<int> a)
        {
            for (var i = 5; i >= 0; i--)
            {
                if (a[i] < 9)
                {
                    a[i]++;
                    return;
                }
                a[i] = 0;
            }
        }
    }
}
