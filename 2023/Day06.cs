namespace _2023
{
    public class Day06
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(288, SolvePart1(ParseInput([
            "Time:      7  15   30",
            "Distance:  9  40  200"
                ])));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(252000, SolvePart1(ParseInput(File.ReadAllLines("input/day06.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(71503, SolvePart2(ParseInputPart2([
            "Time:      7  15   30",
            "Distance:  9  40  200"
                ])));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(36992486, SolvePart2(ParseInputPart2(File.ReadAllLines("input/day06.txt"))));
        }

        private static int SolvePart1(IEnumerable<Race> input)
        {
            int result = 1;
            foreach (var race in input)
            {
                for (var waitTime = 0; waitTime < race.Time; waitTime++)
                {
                    if (waitTime * (race.Time - waitTime) > race.RecordDistance)
                    {
                        var ways = race.Time - 2 * waitTime + 1;
                        result *= ways;
                        break;
                    }
                }
            }
            return result;
        }

        private static int SolvePart2((UInt64 time, UInt64 recordDistance) input)
        {
            var raceTime = input.time;
            var recordDistance = input.recordDistance;
            /* find a wait time that's close, but under the record */
            UInt64 waitTime = raceTime / 2;
            UInt64 prevWaitTime = waitTime;
            bool isUnderTime = false;
            while (true)
            {
                var distance = waitTime * (raceTime - waitTime);
                if (distance > recordDistance)
                {
                    if (isUnderTime)
                    {
                        waitTime = prevWaitTime;
                        break;
                    }
                    isUnderTime = false;
                    waitTime /= 2;
                }
                else
                {
                    prevWaitTime = waitTime;
                    waitTime += waitTime / 2;
                    isUnderTime = true;
                }
            }

            // iterate until we actually find the first time that's over the record.
            for (; waitTime < raceTime; waitTime++)
            {
                if (waitTime * (raceTime - waitTime) > recordDistance)
                {
                    return (int)(raceTime - 2 * waitTime + 1);
                }
            }

            throw new Exception("Should not happen");
        }

        private static IEnumerable<Race> ParseInput(IEnumerable<string> input)
        {
            var inputArray = input.ToArray();
            var times = inputArray[0].Split(':')[1].Trim().Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();
            var distances = inputArray[1].Split(':')[1].Trim().Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();
            return times.Select((t, idx) => new Race(t, distances[idx]));
        }

        private static (UInt64 time, UInt64 recordDistance) ParseInputPart2(IEnumerable<string> input)
        {
            var inputArray = input.ToArray();
            var time = UInt64.Parse(inputArray[0].Split(':')[1].Trim().Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Aggregate((a, b) => a + b));
            var recordDistance = UInt64.Parse(inputArray[1].Split(':')[1].Trim().Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Aggregate((a, b) => a + b));
            return (time, recordDistance);
        }

        private record Race(int Time, int RecordDistance);
    }
}
