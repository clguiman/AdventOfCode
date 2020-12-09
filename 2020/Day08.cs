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
            var asm = new AsmInterpreter(input);
            try
            {
                asm.Run(detectInfiniteLoop: true);
            }
            catch (AsmInterpreter.InfiniteLoopException)
            {
                return asm.Acc;
            }
            return 0;
        }

        private static int Part2(IEnumerable<string> input)
        {
            var instructions = input.Select(i => AsmInterpreter.DecodedInstruction.Decode(i)).ToArray();

            for (var i = 0; i < instructions.Length; i++)
            {
                if (instructions[i].Op == AsmInterpreter.Operation.Jmp || instructions[i].Op == AsmInterpreter.Operation.Nop)
                {
                    var cpy = new AsmInterpreter.DecodedInstruction[instructions.Length];
                    instructions.CopyTo(cpy, 0);
                    cpy[i] = new AsmInterpreter.DecodedInstruction()
                    {
                        Value = cpy[i].Value,
                        Op = (cpy[i].Op == AsmInterpreter.Operation.Jmp) ? AsmInterpreter.Operation.Nop : AsmInterpreter.Operation.Jmp
                    };

                    var asm = new AsmInterpreter(cpy);
                    try
                    {
                        asm.Run(detectInfiniteLoop: true);
                        return asm.Acc;
                    }
                    catch (AsmInterpreter.InfiniteLoopException)
                    {

                    }
                }
            }
            return 0;
        }
    }
}
