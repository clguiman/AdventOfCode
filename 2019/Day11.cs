using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace _2019
{
    public class Day11
    {
        [Fact]
        public async Task Test1Async()
        {
            var panels = new Dictionary<(int X, int Y), long>();
            await PaintAsync(GetRobot(), panels);
            Assert.Equal(2293, panels.Count);
        }

        [Fact]
        public async Task Test2Async()
        {
            var panels = new Dictionary<(int X, int Y), long> { { (0, 0), 1 } };
            await PaintAsync(GetRobot(), panels);

            var leftEdge = panels.Min(p => p.Key.X);
            var rightEdge = panels.Max(p => p.Key.X);
            var topEdge = panels.Min(p => p.Key.Y);
            var bottomEdge = panels.Max(p => p.Key.Y);

            var registration = Enumerable.Range(leftEdge, rightEdge - leftEdge + 1)
                .SelectMany(x => Enumerable.Range(topEdge, bottomEdge - topEdge + 1).Select(y => (x, y)))
                .Select(i => (i, panels.ContainsKey(i) ? panels[i] : 0))
                .GroupBy(x => x.i.y)
                .Select(x => string.Concat(x.Select(t => t.Item2 == 0 ? ' ' : 'X')))
                .ToArray();

            Assert.Equal("  XX  X  X X     XX  XXX  XXX   XX  X      ", registration[0]);
            Assert.Equal(" X  X X  X X    X  X X  X X  X X  X X      ", registration[1]);
            Assert.Equal(" X  X XXXX X    X    X  X X  X X  X X      ", registration[2]);
            Assert.Equal(" XXXX X  X X    X    XXX  XXX  XXXX X      ", registration[3]);
            Assert.Equal(" X  X X  X X    X  X X    X X  X  X X      ", registration[4]);
            Assert.Equal(" X  X X  X XXXX  XX  X    X  X X  X XXXX   ", registration[5]);
        }

        private static IntCodeEmulator GetRobot()
        {
            return new IntCodeEmulator(File.ReadAllText("input/day11.txt").Split(',').Select(long.Parse).ToArray(), true);
        }

        private static async Task PaintAsync(IntCodeEmulator robot, Dictionary<(int X, int Y), long> panels)
        {
            var position = (X: 0, Y: 0);
            var outputState = RobotOutputState.WaitingPaintColor;
            var direction = (X: 0, Y: -1);

            await robot.RunAsync(new IntCodeEmulator.SyncIO(
                () => panels.TryGetValue(position, out var color) ? color : 0,
                (value) =>
                {
                    if (outputState == RobotOutputState.WaitingPaintColor)
                    {
                        if (!panels.ContainsKey(position))
                        {
                            panels.Add(position, value);
                        }
                        else
                        {
                            panels[position] = value;
                        }
                        outputState = RobotOutputState.WaitingDirection;
                    }
                    else
                    {
                        direction = RotateDirection(direction, value == 1);
                        position = (position.X + direction.X, position.Y + direction.Y);
                        outputState = RobotOutputState.WaitingPaintColor;
                    }
                }), default);
        }

        private static (int X, int Y) RotateDirection((int X, int Y) direction, bool right) => right switch
        {
            true => (-1 * direction.Y, direction.X),
            false => (direction.Y, -1 * direction.X),
        };

        private enum RobotOutputState
        {
            WaitingPaintColor,
            WaitingDirection
        }
    }
}
