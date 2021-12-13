using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace _2019
{
    public class Day21
    {
        [Fact]
        public async Task Part1TestAsync()
        {
            var droid = new ASCIIComputer(File.ReadAllText("input/day21.txt").Split(',').Select(long.Parse).ToArray(), resetable: true);
            Assert.Equal(19349939, await Part1Async(droid));
        }

        [Fact]
        public async Task Part2TestAsync()
        {
            var droid = new ASCIIComputer(File.ReadAllText("input/day21.txt").Split(',').Select(long.Parse).ToArray(), resetable: true);
            Assert.Equal(1142412777, await Part2Async(droid));
        }

        private static async Task<long> Part1Async(ASCIIComputer droid)
        {
            var commands = new[] {
                "NOT C J",
                "AND D J",
                "NOT A T",
                "OR T J",
                "WALK"
            };
            long ret = 0;
            var idx = 0;
            await droid.RunAsync(new ASCIIComputer.SyncIO(
                () => commands[idx++],
                (_) => { },
                (longVal) => { ret = longVal; }
            ), default);
            return ret;
        }

        private static async Task<long> Part2Async(ASCIIComputer droid)
        {
            var commands = new[] {
                "NOT B J",
                "NOT C T",
                "OR T J",
                "AND D J",
                "AND H J",
                "NOT A T",
                "OR T J",
                "RUN"
            };
            long ret = 0;
            var idx = 0;
            await droid.RunAsync(new ASCIIComputer.SyncIO(
                () => commands[idx++],
                (_) => { },
                (longVal) => { ret = longVal; }
            ), default);
            return ret;
        }

    }
}
