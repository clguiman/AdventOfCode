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
            public Task<long> ReadAsync(CancellationToken cts);
            public Task WriteAsync(long value, CancellationToken cts);
        }

        public IntCodeEmulator(long[] memory, bool useLargeMemoryMode = false, bool resetable = false)
        {
            this.memory = new InfiniteMemory<long>(memory, useLargeMemoryMode, resetable);
        }

        public void Reset()
        {
            memory.Reset();
        }

        public void Run() => Run(new AsyncQueueIO(new AsyncQueue<long>(), new AsyncQueue<long>()));

        public void Run(ReadOnlySpan<long> input, List<long> output)
        {
            var inputQueue = new AsyncQueue<long>();
            var outputQueue = new AsyncQueue<long>();
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
            long instructionPtr = 0;
            while (instructionPtr < memory.Length)
            {
                if (cts.IsCancellationRequested)
                {
                    return;
                }
                var opCode = DecodedInstruction.DecodeInstruction(memory.GetAt(instructionPtr));
                var operand1 = memory.GetAt(instructionPtr + 1); //(instructionPtr + 1 < memory.Length) ? memory[instructionPtr + 1] : 0;
                var operand2 = memory.GetAt(instructionPtr + 2);//(instructionPtr + 2 < memory.Length) ? memory[instructionPtr + 2] : 0;
                var destination = memory.GetAt(instructionPtr + 3);//(instructionPtr + 3 < memory.Length) ? memory[instructionPtr + 3] : 0;
                switch (opCode.Code)
                {
                    case OpCode.Add:
                        WriteOperand(destination, opCode.Param3Mode, ReadOperand(operand1, opCode.Param1Mode) + ReadOperand(operand2, opCode.Param2Mode));
                        break;
                    case OpCode.Multiply:
                        WriteOperand(destination, opCode.Param3Mode, ReadOperand(operand1, opCode.Param1Mode) * ReadOperand(operand2, opCode.Param2Mode));
                        break;
                    case OpCode.ReadInput:
                        WriteOperand(operand1, opCode.Param1Mode, await io.ReadAsync(cts));
                        break;
                    case OpCode.WriteOutput:
                        await io.WriteAsync(ReadOperand(operand1, opCode.Param1Mode), cts);
                        break;
                    case OpCode.LessThan:
                        WriteOperand(destination, opCode.Param3Mode, ReadOperand(operand1, opCode.Param1Mode) < ReadOperand(operand2, opCode.Param2Mode) ? 1 : 0);
                        break;
                    case OpCode.Equals:
                        WriteOperand(destination, opCode.Param3Mode, ReadOperand(operand1, opCode.Param1Mode) == ReadOperand(operand2, opCode.Param2Mode) ? 1 : 0);
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
                    case OpCode.RelativeBaseOffset:
                        relativeBase += ReadOperand(operand1, opCode.Param1Mode);
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

        public long ReadMemory(long address) => memory.GetAt(address);

        public void WriteMemory(long address, long value)
        {
            memory.SetAt(address, value);
        }

        private long ReadOperand(long operand, ParameterMode mode) => mode switch
        {
            ParameterMode.Position => memory.GetAt(operand),
            ParameterMode.Immediate => operand,
            ParameterMode.Relative => memory.GetAt(operand + relativeBase),
            _ => throw new Exception("Invalid parameter mode!"),
        };

        private void WriteOperand(long operand, ParameterMode mode, long value)
        {
            switch (mode)
            {
                case ParameterMode.Position:
                    memory.SetAt(operand, value);
                    break;
                case ParameterMode.Relative:
                    memory.SetAt(operand + relativeBase, value);
                    break;
                default:
                    throw new Exception("Invalid parameter mode!");
            }
        }

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
            RelativeBaseOffset = 9,
            Halt = 99
        }

        private enum ParameterMode
        {
            Position = 0,
            Immediate = 1,
            Relative = 2,
        }

        private struct DecodedInstruction
        {
            public OpCode Code { get; init; }
            public int Size { get; init; }
            public ParameterMode Param1Mode { get; init; }
            public ParameterMode Param2Mode { get; init; }
            public ParameterMode Param3Mode { get; init; }

            public static DecodedInstruction DecodeInstruction(long rawData) => new DecodedInstruction()
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
                    OpCode.ReadInput or OpCode.WriteOutput or OpCode.RelativeBaseOffset => 2,
                    OpCode.Halt => 1,
                    _ => throw new ArgumentException("Invalid op code!"),
                };
            }
        }

        private readonly InfiniteMemory<long> memory;

        private long relativeBase = 0;

        public sealed class AsyncQueueIO : IAsyncIO
        {
            public AsyncQueueIO(AsyncQueue<long> inputQueue, AsyncQueue<long> outputQueue)
            {
                input = inputQueue;
                output = outputQueue;
            }
            public Task<long> ReadAsync(CancellationToken cts) => input.DequeueAsync(cts);

            public Task WriteAsync(long value, CancellationToken cts)
            {
                output.Enqueue(value);
                return Task.FromResult(true);
            }

            private readonly AsyncQueue<long> input;
            private readonly AsyncQueue<long> output;
        }

        public sealed class SyncIO : IAsyncIO
        {
            public delegate long OnRead();
            public delegate void OnWrite(long value);

            public SyncIO(OnRead readFunc, OnWrite writeFunc)
            {
                input = readFunc;
                output = writeFunc;
            }
            public Task<long> ReadAsync(CancellationToken cts) => Task.FromResult(input());

            public Task WriteAsync(long value, CancellationToken cts)
            {
                output(value);
                return Task.FromResult(false);
            }

            private readonly OnRead input;
            private readonly OnWrite output;
        }
    }

    internal class InfiniteMemory<T>
    {
        public InfiniteMemory(T[] memory, bool useLargeMemoryMode = false, bool resetable = false)
        {
            this._contigousMemory = new T[useLargeMemoryMode ?  1024 * 1024 : memory.Length];
            memory.CopyTo(this._contigousMemory, 0);
            if (resetable)
            {
                this._bkpMemory = new T[this._contigousMemory.Length];
                this._contigousMemory.CopyTo(this._bkpMemory, 0);
            }
            this._sparseMemory = new();
        }

        public long Length => _contigousMemory.LongLength;

        public void Reset()
        {
            if (_bkpMemory == null)
            {
                throw new Exception("The object was not constructed as resetable!");
            }
            this._bkpMemory.CopyTo(this._contigousMemory, 0);
            _sparseMemory.Clear();
        }

        public T GetAt(long idx)
        {
            if (idx < _contigousMemory.Length)
            {
                return _contigousMemory[idx];
            }
            if (_sparseMemory.TryGetValue(idx, out var ret))
            {
                return ret;
            }
            _sparseMemory.Add(idx, default);
            return default;
        }

        public void SetAt(long idx, T value)
        {
            if (idx < _contigousMemory.Length)
            {
                _contigousMemory[idx] = value;
                return;
            }
            if (_sparseMemory.ContainsKey(idx))
            {
                _sparseMemory[idx] = value;
            }
            else
            {
                _sparseMemory.Add(idx, value);
            }
        }

        private readonly T[] _contigousMemory;
        private readonly T[] _bkpMemory;

        private readonly Dictionary<long, T> _sparseMemory;
    }
}
