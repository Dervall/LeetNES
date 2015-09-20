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

            Action<byte> write;

            switch (addressMode)
            {
                case AddressingMode.Accumulator:
                    cycles = 2;
                    arg = cpuState.A;
                    write = b => cpuState.A = b;
                    break;
                case AddressingMode.Implied:
                    cycles = 2;
                    arg = 0; // Not used
                    write = b => { };
                    break;
                case AddressingMode.Immediate:
                {
                    var addr = cpuState.Pc + 1;
                    arg = memory[addr];
                    write = b => memory[addr] = b;
                    cycles = 2;
                    break;
                }
                case AddressingMode.Absolute:
                {
                    var addr = memory.ReadShort(cpuState.Pc + 1);
                    arg = memory[addr];
                    write = b => memory[addr] = b;
                    cycles = 4;
                    break;
                }
                case AddressingMode.ZeroPage:
                {
                    var addr = memory[cpuState.Pc + 1];
                    arg = memory[addr];
                    write = b => memory[addr] = b;
                    cycles = 3;
                    break;
                }
                case AddressingMode.ZeroPageXIndexed:
                {
                    var addr = (memory[cpuState.Pc + 1] + cpuState.X) & 0xFF;
                    arg = memory[addr];
                    write = b => memory[addr] = b;
                    cycles = 4;
                    break;
                }
                case AddressingMode.ZeroPageYIndexed:
                {
                    var addr = (memory[cpuState.Pc + 1] + cpuState.Y) & 0xFF;
                    arg = memory[addr];
                    write = b => memory[addr] = b;
                    cycles = 4;
                    break;
                }
                case AddressingMode.AbsoluteX:
                    arg = GetAbsoluteOffsetArg(cpuState, memory, cpuState.X, out cycles, out write);
                    break;
                case AddressingMode.AbsoluteY:
                    arg = GetAbsoluteOffsetArg(cpuState, memory, cpuState.Y, out cycles, out write);
                    break;
                case AddressingMode.XIndexedIndirect:
                {
                    var addr = memory.ReadShort((memory[cpuState.Pc + 1] + cpuState.X) & 0xFF);
                    arg = memory[addr];
                    write = b => memory[addr] = b;
                    cycles = 6;
                    break;
                }
                case AddressingMode.IndirectYIndexed:
                    var addrPreOffset = memory.ReadShort(memory[cpuState.Pc + 1]);
                    var addrPostOffset = addrPreOffset + cpuState.Y;
                    cycles = 5;
                    arg = memory[addrPostOffset];
                    if ((addrPostOffset & 0xFF00) != (addrPreOffset & 0xFF00))
                    {
                        ++cycles;
                    }
                    write = b => memory[addrPostOffset] = b;
                    break;
                default:
                    throw new Exception("Unimplemented addressing mode");
            }

            InternalExecute(cpuState, memory, arg, write, ref cycles);

            cpuState.Pc += addressMode.InstructionSize();

            return cycles;
        }

        private static byte GetAbsoluteOffsetArg(CpuState cpuState, IMemory memory, byte offset, out int cycles, out Action<byte> write )
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
            write = b => memory[offsetAddress] = b;
            return arg;
        }

        protected abstract void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles);
    }
}