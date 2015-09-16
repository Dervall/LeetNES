using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public abstract class BaseInstruction : IInstruction
    {
        public abstract string Mnemonic { get; }
        public abstract IDictionary<byte, AddressingMode> Variants { get; }

        public int Execute(Cpu.State cpuState, IMemory memory)
        {
            var addressMode = Variants[memory[cpuState.Pc]];
            byte arg;
            int cycles;

            switch (addressMode)
            {
                case AddressingMode.Implied:
                    cycles = 2;
                    arg = 0; // Not used
                    break;
                case AddressingMode.Immediate:
                    arg = memory[cpuState.Pc + 1];
                    cycles = 2;
                    break;
                case AddressingMode.Absolute:
                    arg = memory[memory.ReadShort(cpuState.Pc + 1)];
                    cycles = 4;
                    break;
                default:
                    throw new Exception("Unimplemented addressing mode");
            }

            InternalExecute(cpuState, memory, arg, ref cycles);

            cpuState.Pc += addressMode.InstructionSize();

            return cycles;
        }

        protected abstract void InternalExecute(Cpu.State cpuState, IMemory memory, byte arg, ref int cycles);
    }
}