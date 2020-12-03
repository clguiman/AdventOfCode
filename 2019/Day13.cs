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
            await PlayAsync(GetGame(), screen);
            Assert.Equal(376, screen.Count(x => x.Value == 2));
        }

        [Fact]
        public async Task Test2Async()
        {
            var screen = new Dictionary<(long X, long Y), long>();
            var game = GetGame();
            game.WriteMemory(0, 2);
            Assert.Equal(18509, await PlayAsync(game, screen));
        }

        private static IntCodeEmulator GetGame()
        {
            return new IntCodeEmulator(File.ReadAllText("input/day13.txt").Split(',').Select(long.Parse).ToArray(), true);
        }

        private static async Task<long> PlayAsync(IntCodeEmulator game, Dictionary<(long X, long Y), long> screen)
        {
            var outputState = OutputState.WaitX;
            long x = 0;
            long y = 0;
            long score = 0;
            var ballPosition = (X: 0L, Y: 0L);
            var paddlePosition = (X: 0L, Y: 0L);

            await game.RunAsync(new IntCodeEmulator.SyncIO(
                () => paddlePosition.X > ballPosition.X ? -1 : paddlePosition.X == ballPosition.X ? 0 : 1,
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
                            if (x == -1 && y == 0)
                            {
                                score = value;
                            }
                            else if (value == 4)
                            {
                                ballPosition = (x, y);
                            }
                            else if (value == 3)
                            {
                                paddlePosition = (x, y);
                            }

                            if (screen.ContainsKey((x, y)))
                            {
                                screen[(x, y)] = value;
                            }
                            else
                            {
                                screen.Add((x, y), value);
                            }

                            outputState = OutputState.WaitX;
                            break;
                        default: throw new Exception("invalid state");
                    }
                }), default);
            return score;
        }

        private enum OutputState
        {
            WaitX,
            WaitY,
            WaitTile
        }
    }
}
