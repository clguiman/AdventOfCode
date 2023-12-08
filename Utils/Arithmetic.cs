using System.Numerics;

namespace Utils
{
    public static class Arithmetic
    {
        public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
        {
            if (a < b)
            {
                (a, b) = (b, a);
            }
            var result = a % b;
            while (result != T.Zero)
            {
                a = b;
                b = result;
                result = a % b;
            }
            return b;
        }

        public static T LeastCommonMultiple<T>(T a, T b) where T : INumber<T> => (a * b) / GreatestCommonDivisor(a, b);
    }
}
