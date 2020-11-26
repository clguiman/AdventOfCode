using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _2019
{
    internal class IntCodeEmulator
    {
        public interface IAsyncIO
        {
            public Task<int> ReadAsync(CancellationToken cts);
            public Task WriteAsync(int value, CancellationToken cts);
        }

        public IntCodeEmulator(int[] memory)
        {
            this.memory = new int[memory.Length];
            memory.CopyTo(this.memory, 0);
        }

        public void Run() => Run(new AsyncQueueIO(new AsyncQueue<int>(), new AsyncQueue<int>()));

        public void Run(ReadOnlySpan<int> input, List<int> output)
        {
            var inputQueue = new AsyncQueue<int>();
            var outputQueue = new AsyncQueue<int>();
            foreach (var i in input)
            {
                inputQueue.Enqueue(i);
            }

            Run(new AsyncQueueIO(inputQueue, outputQueue));

            output.Clear();
            while (outputQueue.TryDequeue(out var v))
            {
                output.Add(v);
            }
        }

        public void Run(IAsyncIO io)
        {
            new JoinableTaskFactory(new JoinableTaskContext()).Run(async () => await RunAsync(io, default));
        }

        public async Task RunAsync(IAsyncIO io, CancellationToken cts)
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
                        memory[operand1] = await io.ReadAsync(cts);
                        break;
                    case OpCode.WriteOutput:
                        await io.WriteAsync(ReadOperand(operand1, opCode.Param1Mode), cts);
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

        public sealed class AsyncQueueIO : IAsyncIO
        {
            public AsyncQueueIO(AsyncQueue<int> inputQueue, AsyncQueue<int> outputQueue)
            {
                input = inputQueue;
                output = outputQueue;
            }
            public Task<int> ReadAsync(CancellationToken cts) => input.DequeueAsync(cts);

            public Task WriteAsync(int value, CancellationToken cts)
            {
                output.Enqueue(value);
                return Task.FromResult(true);
            }

            private readonly AsyncQueue<int> input;
            private readonly AsyncQueue<int> output;
        }
    }
}
