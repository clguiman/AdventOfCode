using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace _2019
{
    public class Day13
    {
        [Fact]
        public async Task Test1Async()
        {
            var screen = new Dictionary<(long X, long Y), long>();
            await PlayAsync(GetRobot(), screen);
            Assert.Equal(376, screen.Count(x => x.Value == 2));
        }

        private static IntCodeEmulator GetRobot()
        {
            return new IntCodeEmulator(File.ReadAllText("input/day13.txt").Split(',').Select(long.Parse).ToArray(), true);
        }

        private static async Task PlayAsync(IntCodeEmulator robot, Dictionary<(long X, long Y), long> screen)
        {
            var outputState = OutputState.WaitX;
            long x = 0;
            long y = 0;

            await robot.RunAsync(new IntCodeEmulator.SyncIO(
                () => throw new Exception("Read isn't expected"),
                (value) =>
                {
                    switch (outputState)
                    {
                        case OutputState.WaitX:
                            x = value;
                            outputState = OutputState.WaitY;
                            break;
                        case OutputState.WaitY:
                            y = value;
                            outputState = OutputState.WaitTile;
                            break;
                        case OutputState.WaitTile:
                            screen.Add((x, y), value);
                            outputState = OutputState.WaitX;
                            break;
                        default: throw new Exception("invalid state");
                    }
                }), default);
        }

        private enum OutputState
        {
            WaitX,
            WaitY,
            WaitTile
        }
    }
}
