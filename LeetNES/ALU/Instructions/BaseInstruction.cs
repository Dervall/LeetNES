using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public abstract class BaseInstruction : IInstruction
    {
        public abstract string Mnemonic { get; }
        public abstract IDictionary<byte, AddressingMode> Variants { get; }

        public int Execute(CpuState cpuState, IMemory memory)
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
                case AddressingMode.ZeroPage:
                    arg = memory[memory[cpuState.Pc + 1]];
                    cycles = 3;
                    break;
                case AddressingMode.ZeroPageXIndexed:
                    arg = memory[(memory[cpuState.Pc + 1] + cpuState.X) & 0xFF];
                    cycles = 4;
                    break;
                case AddressingMode.AbsoluteX:
                    arg = GetAbsoluteOffsetArg(cpuState, memory, cpuState.X, out cycles);
                    break;
                case AddressingMode.AbsoluteY:
                    arg = GetAbsoluteOffsetArg(cpuState, memory, cpuState.Y, out cycles);
                    break;
                case AddressingMode.XIndexedIndirect:
                    arg = memory[memory.ReadShort((memory[cpuState.Pc + 1] + cpuState.X) & 0xFF)];
                    cycles = 6;
                    break;
                case AddressingMode.IndirectYIndexed:
                    var addrPreOffset = memory.ReadShort(memory[cpuState.Pc + 1]);
                    var addrPostOffset = addrPreOffset + cpuState.Y;
                    cycles = 5;
                    arg = memory[addrPostOffset];
                    if ((addrPostOffset & 0xFF00) != (addrPreOffset & 0xFF00))
                    {
                        ++cycles;
                    }
                    break;
                default:
                    throw new Exception("Unimplemented addressing mode");
            }

            InternalExecute(cpuState, memory, arg, ref cycles);

            cpuState.Pc += addressMode.InstructionSize();

            return cycles;
        }

        private static byte GetAbsoluteOffsetArg(CpuState cpuState, IMemory memory, byte offset, out int cycles)
        {
            byte arg;
            var address = memory.ReadShort(cpuState.Pc + 1);
            var offsetAddress = address + offset;
            arg = memory[offsetAddress];
            cycles = 4;
            if ((offsetAddress & 0xFF00) != (address & 0xFF00))
            {
                ++cycles;
            }
            return arg;
        }

        protected abstract void InternalExecute(CpuState cpuState, IMemory memory, byte arg, ref int cycles);
    }
}