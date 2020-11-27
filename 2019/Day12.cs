using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace _2019
{
    public class Day12
    {
        [Fact]
        public void Test1()
        {
            var moons = new Moon[] { new(new(-1, 0, 2)), new(new(2, -10, -7)), new(new(4, -8, 8)), new(new(3, 5, -1)) };

            Step(moons);
            moons[0].AssertState(2, -1, 1, 3, -1, -1);
            moons[1].AssertState(3, -7, -4, 1, 3, 3);
            moons[2].AssertState(1, -7, 5, -3, 1, -3);
            moons[3].AssertState(2, 2, 0, -1, -3, 1);
            for (var i = 1; i < 10; i++)
            {
                Step(moons);
            }

            moons[0].AssertState(2, 1, -3, -3, -2, 1);
            moons[1].AssertState(1, -8, 0, -1, 1, 3);
            moons[2].AssertState(3, -6, 1, 3, 2, -3);
            moons[3].AssertState(2, 0, 4, 1, -1, -1);

            Assert.Equal(179, moons.Sum(m => m.Energy));
        }

        [Fact]
        public void Test2()
        {
            var moons = new Moon[] { new(new(-8, -10, 0)), new(new(5, 5, 10)), new(new(2, -7, 3)), new(new(9, -8, -3)) };
            for (var i = 0; i < 100; i++)
            {
                Step(moons);
            }
            Assert.Equal(1940, moons.Sum(m => m.Energy));
        }

        [Fact]
        public void Test3()
        {
            var moons = new Moon[] { new(new(5, 4, 4)), new(new(-11, -11, -3)), new(new(0, 7, 0)), new(new(-13, 2, 10)) };
            for (var i = 0; i < 1000; i++)
            {
                Step(moons);
            }
            Assert.Equal(10845, moons.Sum(m => m.Energy));
        }

        [Fact]
        public void Test5()
        {
            var moons = new Moon[] { new(new(-1, 0, 2)), new(new(2, -10, -7)), new(new(4, -8, 8)), new(new(3, 5, -1)) };
            Assert.Equal(2772, Solution2(moons));
        }

        [Fact]
        public void Test6()
        {
            var moons = new Moon[] { new(new(-8, -10, 0)), new(new(5, 5, 10)), new(new(2, -7, 3)), new(new(9, -8, -3)) };
            Assert.Equal(4686774924, Solution2(moons));
        }
        [Fact]
        public void Test7()
        {
            var moons = new Moon[] { new(new(5, 4, 4)), new(new(-11, -11, -3)), new(new(0, 7, 0)), new(new(-13, 2, 10)) };
            Assert.Equal(551272644867044, Solution2(moons));
        }

        private static void Step(Moon[] moons)
        {
            for (var i = 0; i < moons.Length; i++)
            {
                for (var j = i + 1; j < moons.Length; j++)
                {
                    var delta = moons[i].ComputeVelocityChange(moons[j]);
                    moons[i].Velocity = new Dimensions(moons[i].Velocity.X + delta.X, moons[i].Velocity.Y + delta.Y, moons[i].Velocity.Z + delta.Z);
                    moons[j].Velocity = new Dimensions(moons[j].Velocity.X - delta.X, moons[j].Velocity.Y - delta.Y, moons[j].Velocity.Z - delta.Z);
                }
                moons[i].Position = new Dimensions(moons[i].Position.X + moons[i].Velocity.X, moons[i].Position.Y + moons[i].Velocity.Y, moons[i].Position.Z + moons[i].Velocity.Z);
            }
        }

        private long Solution2(Moon[] moons)
        {
            var initialState = new Moon[moons.Length];
            moons.CopyTo(initialState, 0);

            var posXCycle = 0;
            var posYCycle = 0;
            var posZCycle = 0;
            for (var count = 1; posXCycle == 0 || posYCycle == 0 || posZCycle == 0; count++)
            {
                Step(moons);

                if (posXCycle == 0 &&
                    !Enumerable.Range(0, moons.Length)
                    .Select(i => moons[i].Position.X == initialState[i].Position.X && moons[i].Velocity.X == initialState[i].Velocity.X)
                    .Any(x => x == false))
                {
                    posXCycle = count;
                }
                if (posYCycle == 0 &&
                    !Enumerable.Range(0, moons.Length)
                    .Select(i => moons[i].Position.Y == initialState[i].Position.Y && moons[i].Velocity.Y == initialState[i].Velocity.Y)
                    .Any(x => x == false))
                {
                    posYCycle = count;
                }
                if (posZCycle == 0 &&
                    !Enumerable.Range(0, moons.Length)
                    .Select(i => moons[i].Position.Z == initialState[i].Position.Z && moons[i].Velocity.Z == initialState[i].Velocity.Z)
                    .Any(x => x == false))
                {
                    posZCycle = count;
                }
            }
            return LCM(LCM(posXCycle, posYCycle), posZCycle);
        }

        private static long GCD(long a, long b)
        {
            if (a < b) { var t = b; b = a; a = t; }
            var result = a % b;
            while (result != 0)
            {
                a = b;
                b = result;
                result = a % b;
            }
            return b;
        }

        private static long LCM(long a, long b) => (a * b) / GCD(a, b);

        private record Dimensions(int X, int Y, int Z);

        private struct Moon
        {
            public Dimensions Position { get; set; }
            public Dimensions Velocity { get; set; }

            public Moon(Dimensions initialPosition)
            {
                Position = initialPosition;
                Velocity = new Dimensions(0, 0, 0);
            }

            public int Energy => (Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z)) * (Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z));

            public Dimensions ComputeVelocityChange(Moon other)
            {
                int deltaX = 0, deltaY = 0, deltaZ = 0;
                deltaX = Comparer<int>.Default.Compare(other.Position.X, Position.X);//  Position.X < other.Position.X ? 1 : Position.X > other.Position.X ? -1 : 0;
                deltaY = Comparer<int>.Default.Compare(other.Position.Y, Position.Y);
                deltaZ = Comparer<int>.Default.Compare(other.Position.Z, Position.Z);
                return new Dimensions(deltaX, deltaY, deltaZ);
            }

            public void AssertState(int posX, int posY, int posZ, int velX, int velY, int velZ)
            {
                Assert.Equal(new(posX, posY, posZ), Position);
                Assert.Equal(new(velX, velY, velZ), Velocity);
            }
        }
    }
}
