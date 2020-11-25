using System;

namespace _2019
{
    internal class IntCodeEmulator
    {
        public IntCodeEmulator(int[] memory)
        {
            this.memory = new int[memory.Length];
            memory.CopyTo(this.memory, 0);
        }

        public void Run()
        {
            for (var i = 0; i < memory.Length; i += 4)
            {
                var opCode = memory[i];
                var operand1 = (i + 1 < memory.Length) ? memory[i + 1] : 0;
                var operand2 = (i + 2 < memory.Length) ? memory[i + 2] : 0;
                var destination = (i + 3 < memory.Length) ? memory[i + 3] : 0;
                switch (opCode)
                {
                    case 1:
                        memory[destination] = memory[operand1] + memory[operand2];
                        break;
                    case 2:
                        memory[destination] = memory[operand1] * memory[operand2];
                        break;
                    case 99:
                        return;
                    default:
                        throw new Exception("Invalid op code!");
                }
            }
            throw new Exception("The program didn't halt!");
        }

        public int ReadMemory(int address)
        {
            return memory[address];
        }

        public void WriteMemory(int address, int value)
        {
            memory[address] = value;
        }

        private readonly int[] memory;
    }
}
