using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2022
{
    public class Day05
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("CMZ", Part1(ParseInput(new[] {
                "    [D]    ",
                "[N] [C]    ",
                "[Z] [M] [P]",
                " 1   2   3 ",
                "",
                "move 1 from 2 to 1",
                "move 3 from 1 to 3",
                "move 2 from 2 to 1",
                "move 1 from 1 to 2"
            })));
        }
        [Fact]
        public void Test2()
        {
            Assert.Equal("LBLVVTVLP", Part1(ParseInput(File.ReadAllLines("input/day05.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("MCD", Part2(ParseInput(new[] {
                "    [D]    ",
                "[N] [C]    ",
                "[Z] [M] [P]",
                " 1   2   3 ",
                "",
                "move 1 from 2 to 1",
                "move 3 from 1 to 3",
                "move 2 from 2 to 1",
                "move 1 from 1 to 2"
            })));
        }
        [Fact]
        public void Test4()
        {
            Assert.Equal("TPFFBDRJD", Part2(ParseInput(File.ReadAllLines("input/day05.txt"))));
        }

        private static string Part1((Stack<char>[] stacks, IEnumerable<Instruction> instructions) input)
        {
            foreach (var instruction in input.instructions)
            {
                foreach (var crate in Enumerable.Range(0, instruction.Count).Select(_ => input.stacks[instruction.Source - 1].Pop()))
                {
                    input.stacks[instruction.Destination - 1].Push(crate);
                }
            }
            return new string(input.stacks.Select(s => s.Peek()).ToArray());
        }

        private static string Part2((Stack<char>[] stacks, IEnumerable<Instruction> instructions) input)
        {
            foreach (var instruction in input.instructions)
            {
                foreach (var crate in Enumerable.Range(0, instruction.Count).Select(_ => input.stacks[instruction.Source - 1].Pop()).Reverse())
                {
                    input.stacks[instruction.Destination - 1].Push(crate);
                }
            }
            return new string(input.stacks.Select(s => s.Peek()).ToArray());
        }

        private static (Stack<char>[] stacks, IEnumerable<Instruction> instructions) ParseInput(IEnumerable<string> input)
        {
            var stackCount = input.First().Length / 4 + 1;
            List<char>[] reversedStacks = Enumerable.Range(0, stackCount).Select(_ => new List<char>()).ToArray();
            List<Instruction> instructions = new();

            bool readInstructions = false;
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    readInstructions = true;
                    continue;
                }
                if (readInstructions)
                {
                    instructions.Add(new Instruction(line));
                    continue;
                }

                for (var idx = 0; idx < line.Length; idx += 4)
                {
                    var token = line.Substring(idx, 3).Trim();
                    if (token.Length != 3)
                    {
                        continue;
                    }
                    char crate = token[1];
                    reversedStacks[idx / 4].Add(crate);
                }
            }
            return (reversedStacks.Select(x => new Stack<char>(x.Reverse<char>())).ToArray(), instructions);
        }

        private struct Instruction
        {
            public int Count { get; private set; }
            public int Source { get; private set; }
            public int Destination { get; private set; }


            public Instruction(string instructionStr)
            {
                var tokens = instructionStr.Split(' ');
                Count = int.Parse(tokens[1]);
                Source = int.Parse(tokens[3]);
                Destination = int.Parse(tokens[5]);
            }

        }
    }
}