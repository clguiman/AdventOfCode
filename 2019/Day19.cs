using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace _2019
{
    public class Day19
    {
        [Fact]
        public async Task Part1TestAsync()
        {
            var drone = new IntCodeEmulator(File.ReadAllText("input/day19.txt").Split(',').Select(long.Parse).ToArray(), resetable: true);
            var regionsToVisit = Enumerable.Range(0, 50).SelectMany(x => Enumerable.Range(0, 50).Select(y => (x, y))).ToArray();
            Assert.Equal(223, (await DiscoverBeamAsync(drone, regionsToVisit)).Count(r => r.beam));
        }

        [Fact]
        public async Task Part2TestAsync()
        {
            var drone = new IntCodeEmulator(File.ReadAllText("input/day19.txt").Split(',').Select(long.Parse).ToArray(), resetable: true);

            var curX = 187;
            var curY = 0;
            for (; ; curX++, curY++)
            {
                if (3 == (await DiscoverBeamAsync(drone, new[] { (curX, curY), (curX + 99, curY), (curX + 99, curY + 99) })).Count(r => r.beam))
                {
                    break;
                }
            }
            var result = curX * 10000 + curY;
            Assert.Equal(9480761, result);
        }

        private static async Task<(int x, int y, bool beam)[]> DiscoverBeamAsync(IntCodeEmulator drone, (int x, int y)[] regionsToVisit)
        {
            var ret = regionsToVisit.Select(r => (r.x, r.y, false)).ToArray();
            var idx = 0;
            while (idx < regionsToVisit.Length * 2)
            {
                drone.Reset();
                await drone.RunAsync(new IntCodeEmulator.SyncIO(
                    () =>
                    {
                        idx++;
                        if (idx % 2 == 1)
                        {
                            return regionsToVisit[(idx - 1) / 2].x;
                        }
                        else
                        {
                            return regionsToVisit[(idx - 1) / 2].y;
                        }
                    },
                    (value) =>
                    {
                        if (value == 1)
                        {
                            ret[(idx / 2) - 1].Item3 = true;
                        }
                    }
                ), default);
            }

            return ret;
        }

    }
}
