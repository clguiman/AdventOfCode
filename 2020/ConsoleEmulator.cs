using System;
using System.Collections.Generic;
using System.Linq;

namespace _2020
{
    internal class ConsoleEmulator
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

        public ConsoleEmulator(IEnumerable<string> instructions) : this(instructions.Select(i => DecodedInstruction.Decode(i)))
        {
        }

        public ConsoleEmulator(IEnumerable<DecodedInstruction> instructions)
        {
            this.instructions = instructions.ToArray();
        }

        public void Run(bool detectInfiniteLoop)
        {
            var executedInstructions = new HashSet<int>();
            while (ip < instructions.Length)
            {
                if (detectInfiniteLoop)
                {
                    if (executedInstructions.Contains(ip))
                    {
                        throw new InfiniteLoopException($"Instruction on position {ip} ({instructions[ip]}) was already executed!");
                    }
                    executedInstructions.Add(ip);
                }

                Step(instructions[ip]);
            }
        }

        private void Step(DecodedInstruction instr)
        {
            switch (instr.Op)
            {
                case Operation.Acc:
                    Acc += instr.Value;
                    ip++;
                    break;
                case Operation.Jmp:
                    ip += instr.Value;
                    break;
                default:
                    ip++;
                    break;
            }
        }

        public int Acc { get; private set; }

        private int ip;

        private readonly DecodedInstruction[] instructions;
    }
}
