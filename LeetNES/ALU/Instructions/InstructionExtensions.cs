using System;
using System.Runtime.CompilerServices;

namespace LeetNES.ALU.Instructions
{
    public static class InstructionExtensions
    {
        public static string Disassemble(this IInstruction instruction, byte[] args, IMemory memory, CpuState cpuState)
        {
            switch (instruction.Variants[args[0]])
            {
                case AddressingMode.Accumulator:
                    return string.Format("{0} A", instruction.Mnemonic);
                case AddressingMode.Absolute:
                    if (instruction is JMP || instruction is JSR)
                    {
                        return string.Format("{0} ${1:X2}{2:X2}", instruction.Mnemonic, args[2], args[1]);
                    }
                    else
                    {
                        return string.Format("{0} ${1:X2}{2:X2} = {3:X2}", instruction.Mnemonic, args[2], args[1], memory[(args[2] << 8) | args[1]]);
                    }
                case AddressingMode.AbsoluteX:
                {
                    ushort addr = (ushort) ((args[2] << 8) | args[1]);
                    addr += cpuState.X;

                    return string.Format("{0} ${1:X2}{2:X2},X @ {3:X4} = {4:X2}", instruction.Mnemonic, args[2], args[1], addr, memory[addr]);
                }
                case AddressingMode.AbsoluteY:
                {
                    ushort addr = (ushort) ((args[2] << 8) | args[1]);
                    addr += cpuState.Y;

                    return string.Format("{0} ${1:X2}{2:X2},Y @ {3:X4} = {4:X2}", instruction.Mnemonic, args[2], args[1], addr, memory[addr]);
                }
                case AddressingMode.Immediate:
                    return string.Format("{0} #${1:X2}", instruction.Mnemonic, args[1]);
                case AddressingMode.Implied:
                    return instruction.Mnemonic;
                case AddressingMode.Indirect:
                    return string.Format("{0} (${1:X2}{2:X2}) = {3:X4}", instruction.Mnemonic, args[2], args[1], memory.ReadShort((args[2] << 8) | args[1]));
                case AddressingMode.XIndexedIndirect:
                {
                    byte addr = (byte) (args[1] + cpuState.X);
                    var finalAddr = memory.ReadZeroPageShort(addr);
                    return string.Format("{0} (${1:X2},X) @ {2:X2} = {3:X4} = {4:X2}", instruction.Mnemonic, args[1],
                        addr, finalAddr, memory[finalAddr]);
                }
                    
                case AddressingMode.IndirectYIndexed:
                {
                    ushort addr = memory.ReadZeroPageShort(args[1]);
                    ushort finalAddr = (ushort) (addr + cpuState.Y);
                    return string.Format("{0} (${1:X2}),Y = {2:X4} @ {3:X4} = {4:X2}", instruction.Mnemonic, args[1],
                        addr, finalAddr, memory[finalAddr]);
                }

                case AddressingMode.Relative:
                {
                    ushort newPc;
                    if ((args[1] & 0x80) != 0)
                    {
                        newPc = (ushort)(cpuState.Pc - (0x100 - args[1]));
                    }
                    else
                    {
                        newPc = (ushort)(cpuState.Pc + args[1]);
                    }
                    return string.Format("{0} ${1:X4}", instruction.Mnemonic, newPc + 2);
                }
                case AddressingMode.ZeroPage:
                    return string.Format("{0} ${1:X2} = {2:X2}", instruction.Mnemonic, args[1], memory[args[1]]);
                case AddressingMode.ZeroPageXIndexed:
                    return string.Format("{0} ${1:X2},X", instruction.Mnemonic, args[1]);
                case AddressingMode.ZeroPageYIndexed:
                    return string.Format("{0} ${1:X2},Y", instruction.Mnemonic, args[1]);
                default:
                    throw new Exception("Disassembly for addressing mode is unimplemented");
            }
        }
    }
}