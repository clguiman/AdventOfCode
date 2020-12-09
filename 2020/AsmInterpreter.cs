using System;
using System.Collections.Generic;
using System.Linq;

namespace _2020
{
    internal class AsmInterpreter
    {
        public enum Operation
        {
            Invalid,
            Acc,
            Jmp,
            Nop,
        }

        public struct DecodedInstruction
        {
            public Operation Op { get; init; }
            public int Value { get; init; }

            public static DecodedInstruction Decode(string instr)
            {
                var tokens = instr.Split(' ');
                var op = (tokens[0]) switch
                {
                    "acc" => Operation.Acc,
                    "jmp" => Operation.Jmp,
                    "nop" => Operation.Nop,
                    _ => throw new ArgumentException($"Invalid Operation: {tokens[0]}"),
                };
                return new DecodedInstruction() { Op = op, Value = int.Parse(tokens[1]) };
            }
        }

        public class InfiniteLoopException : Exception
        {
            public InfiniteLoopException(string msg) : base(msg)
            {
            }
        }

        public AsmInterpreter(IEnumerable<string> instructions) : this(instructions.Select(i => DecodedInstruction.Decode(i)))
        {
        }

        public AsmInterpreter(IEnumerable<DecodedInstruction> instructions)
        {
            this.instructions = instructions.ToArray();
        }

        public void Run(bool detectInfiniteLoop)
        {
            var executedInstructions = new HashSet<int>();
            while (Ip < instructions.Length)
            {
                if (detectInfiniteLoop)
                {
                    if (executedInstructions.Contains(Ip))
                    {
                        throw new InfiniteLoopException($"Instruction on position {Ip} ({instructions[Ip]}) was already executed!");
                    }
                    executedInstructions.Add(Ip);
                }

                Step(instructions[Ip]);
            }
        }

        private void Step(DecodedInstruction instr)
        {
            switch (instr.Op)
            {
                case Operation.Acc:
                    Acc += instr.Value;
                    Ip++;
                    break;
                case Operation.Jmp:
                    Ip += instr.Value;
                    break;
                default:
                    Ip++;
                    break;
            }
        }

        public int Acc { get; private set; }

        public int Ip { get; private set; }

        private readonly DecodedInstruction[] instructions;
    }
}
