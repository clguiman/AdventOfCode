using System.IO;
using Xunit;

namespace _2022
{
    public class Day06
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(7, Solve("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 4));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(5, Solve("bvwbjplbgvbhsrlpgdmjqwftvncz", 4));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(6, Solve("nppdvjthqldpwncqszvftbrmjlhg", 4));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(10, Solve("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 4));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(11, Solve("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 4));
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal(1655, Solve(File.ReadAllText("input/day06.txt"), 4));
        }

        [Fact]
        public void Test7()
        {
            Assert.Equal(19, Solve("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 14));
        }

        [Fact]
        public void Test8()
        {
            Assert.Equal(23, Solve("bvwbjplbgvbhsrlpgdmjqwftvncz", 14));
        }

        [Fact]
        public void Test9()
        {
            Assert.Equal(23, Solve("nppdvjthqldpwncqszvftbrmjlhg", 14));
        }

        [Fact]
        public void Test10()
        {
            Assert.Equal(29, Solve("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 14));
        }

        [Fact]
        public void Test11()
        {
            Assert.Equal(26, Solve("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 14));
        }

        [Fact]
        public void Test12()
        {
            Assert.Equal(2665, Solve(File.ReadAllText("input/day06.txt"), 14));
        }

        private static int Solve(string buffer, int distinctCount)
        {
            var lastOccurenceIndeces = new int['z' - 'a' + 1];
            var duplicateDistances = new int[buffer.Length];

            for (var idx = 0; idx < buffer.Length; idx++)
            {
                var lastOccurenceIdx = lastOccurenceIndeces[buffer[idx] - 'a'];
                lastOccurenceIndeces[buffer[idx] - 'a'] = idx;
                if (idx == 0)
                {
                    duplicateDistances[0] = 0;
                    continue;
                }

                if (lastOccurenceIdx == 0)
                {
                    duplicateDistances[idx] = buffer.Length;
                }
                else
                {
                    duplicateDistances[idx] = idx - lastOccurenceIdx;
                }
            }

            for (var idx = distinctCount - 1; idx < buffer.Length; idx++)
            {
                bool found = true;
                for (var j = idx - distinctCount + 1; j <= idx; j++)
                {
                    if (duplicateDistances[j] < j - idx + distinctCount)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return idx + 1;
                }
            }
            return 0;
        }
    }
}