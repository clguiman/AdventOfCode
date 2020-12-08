using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day08
    {
        [Fact]
        public void Test1()
        {
            var input = new[] {
            "nop +0",
"acc +1",
"jmp +4",
"acc +3",
"jmp -3",
"acc -99",
"acc +1",
"jmp -4",
"acc +6"};
            Assert.Equal(5, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(1563, Part1(File.ReadAllLines("input/day08.txt")));
        }

        [Fact]
        public void Test3()
        {
            var input = new[] {
            "nop +0",
"acc +1",
"jmp +4",
"acc +3",
"jmp -3",
"acc -99",
"acc +1",
"jmp -4",
"acc +6"};
            Assert.Equal(8, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(767, Part2(File.ReadAllLines("input/day08.txt")));
        }

        private static int Part1(IEnumerable<string> input)
        {
            var console = new ConsoleEmulator(input);
            try
            {
                console.Run(detectInfiniteLoop: true);
            }
            catch (ConsoleEmulator.InfiniteLoopException)
            {
                return console.Acc;
            }
            return 0;
        }

        private static int Part2(IEnumerable<string> input)
        {
            var instructions = input.Select(i => ConsoleEmulator.DecodedInstruction.Decode(i)).ToArray();

            for (var i = 0; i < instructions.Length; i++)
            {
                if (instructions[i].Op == ConsoleEmulator.Operation.Jmp || instructions[i].Op == ConsoleEmulator.Operation.Nop)
                {
                    var cpy = new ConsoleEmulator.DecodedInstruction[instructions.Length];
                    instructions.CopyTo(cpy, 0);
                    cpy[i] = new ConsoleEmulator.DecodedInstruction()
                    {
                        Value = cpy[i].Value,
                        Op = (cpy[i].Op == ConsoleEmulator.Operation.Jmp) ? ConsoleEmulator.Operation.Nop : ConsoleEmulator.Operation.Jmp
                    };

                    var console = new ConsoleEmulator(cpy);
                    try
                    {
                        console.Run(detectInfiniteLoop: true);
                        return console.Acc;
                    }
                    catch (ConsoleEmulator.InfiniteLoopException)
                    {

                    }
                }
            }
            return 0;
        }
    }
}
