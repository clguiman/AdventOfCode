using System;
using System.Collections.Generic;
using System.IO;

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
            // ignore I/O
            using var stream = new MemoryStream();
            using var br = new BinaryReader(stream);
            using var bw = new BinaryWriter(stream);
            Run(br, bw);
        }

        public void Run(ReadOnlySpan<int> input, List<int> output)
        {
            output.Clear();
            using var inputStream = new MemoryStream(input.Length * sizeof(int));
            using var inputWriter = new BinaryWriter(inputStream);
            foreach (var i in input)
            {
                inputWriter.Write(i);
            }
            inputStream.Position = 0;

            using var outputStream = new MemoryStream();
            using var br = new BinaryReader(inputStream);
            using var bw = new BinaryWriter(outputStream);
            Run(br, bw);

            outputStream.Position = 0;
            using var outputReader = new BinaryReader(outputStream);
            
            while (outputStream.Position < outputStream.Length)
            {
                output.Add(outputReader.ReadInt32());
            }
        }

        public void Run(BinaryReader inputStream, BinaryWriter outputStream)
        {
            var instructionPtr = 0;
            while (instructionPtr < memory.Length)
            {
                var opCode = DecodedInstruction.DecodeInstruction(memory[instructionPtr]);
                var operand1 = (instructionPtr + 1 < memory.Length) ? memory[instructionPtr + 1] : 0;
                var operand2 = (instructionPtr + 2 < memory.Length) ? memory[instructionPtr + 2] : 0;
                var destination = (instructionPtr + 3 < memory.Length) ? memory[instructionPtr + 3] : 0;
                switch (opCode.Code)
                {
                    case OpCode.Add:
                        memory[destination] = ReadOperand(operand1, opCode.Param1Mode) + ReadOperand(operand2, opCode.Param2Mode);
                        break;
                    case OpCode.Multiply:
                        memory[destination] = ReadOperand(operand1, opCode.Param1Mode) * ReadOperand(operand2, opCode.Param2Mode);
                        break;
                    case OpCode.ReadInput:
                        memory[operand1] = inputStream.ReadInt32();
                        break;
                    case OpCode.WriteOutput:
                        outputStream.Write(ReadOperand(operand1, opCode.Param1Mode));
                        break;
                    case OpCode.LessThan:
                        memory[destination] = ReadOperand(operand1, opCode.Param1Mode) < ReadOperand(operand2, opCode.Param2Mode) ? 1 : 0;
                        break;
                    case OpCode.Equals:
                        memory[destination] = ReadOperand(operand1, opCode.Param1Mode) == ReadOperand(operand2, opCode.Param2Mode) ? 1 : 0;
                        break;
                    case OpCode.JumpIfTrue:
                        if (ReadOperand(operand1, opCode.Param1Mode) != 0)
                        {
                            instructionPtr = ReadOperand(operand2, opCode.Param2Mode) - opCode.Size;
                        }
                        break;
                    case OpCode.JumpIfFalse:
                        if (ReadOperand(operand1, opCode.Param1Mode) == 0)
                        {
                            instructionPtr = ReadOperand(operand2, opCode.Param2Mode) - opCode.Size;
                        }
                        break;
                    case OpCode.Halt:
                        return;
                    default:
                        throw new Exception($"Invalid op code!");
                }
                instructionPtr += opCode.Size;
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

        private int ReadOperand(int operand, ParameterMode mode) => mode switch
        {
            ParameterMode.Position => memory[operand],
            ParameterMode.Immediate => operand,
            _ => throw new Exception("Invalid parameter mode!"),
        };

        private enum OpCode
        {
            Add = 1,
            Multiply = 2,
            ReadInput = 3,
            WriteOutput = 4,
            JumpIfTrue = 5,
            JumpIfFalse = 6,
            LessThan = 7,
            Equals = 8,
            Halt = 99
        }

        private enum ParameterMode
        {
            Position = 0,
            Immediate = 1,
        }

        private struct DecodedInstruction
        {
            public OpCode Code { get; init; }
            public int Size { get; init; }
            public ParameterMode Param1Mode { get; init; }
            public ParameterMode Param2Mode { get; init; }
            public ParameterMode Param3Mode { get; init; }

            public static DecodedInstruction DecodeInstruction(int rawData) => new DecodedInstruction()
            {
                Code = (OpCode)(rawData % 100),
                Size = GetInstructionSize((OpCode)(rawData % 100)),
                Param1Mode = (ParameterMode)((rawData % 1000) / 100),
                Param2Mode = (ParameterMode)((rawData % 10000) / 1000),
                Param3Mode = (ParameterMode)((rawData % 100000) / 10000)
            };

            private static int GetInstructionSize(OpCode code)
            {
                return code switch
                {
                    OpCode.Add or OpCode.Multiply or OpCode.LessThan or OpCode.Equals => 4,
                    OpCode.JumpIfFalse or OpCode.JumpIfTrue => 3,
                    OpCode.ReadInput or OpCode.WriteOutput => 2,
                    OpCode.Halt => 1,
                    _ => throw new ArgumentException("Invalid op code!"),
                };
            }
        }

        private readonly int[] memory;
    }
}
