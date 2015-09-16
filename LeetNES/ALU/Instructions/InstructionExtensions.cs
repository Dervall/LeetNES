using System;

namespace LeetNES.ALU.Instructions
{
    public static class InstructionExtensions
    {
        public static string Disassemble(this IInstruction instruction, byte[] args)
        {
            switch (instruction.Variants[args[0]])
            {
                case AddressingMode.Accumulator:
                    return string.Format("{0} A", instruction.Mnemonic);
                case AddressingMode.Absolute:
                    return string.Format("{0} ${1:X2}{2:X2}", instruction.Mnemonic, args[2], args[1]);
                case AddressingMode.AbsoluteX:
                    return string.Format("{0} ${1:X2}{2:X2},X", instruction.Mnemonic, args[2], args[1]);
                case AddressingMode.AbsoluteY:
                    return string.Format("{0} ${1:X2}{2:X2},Y", instruction.Mnemonic, args[2], args[1]);
                case AddressingMode.Immediate:
                    return string.Format("{0} #${1:X2}", instruction.Mnemonic, args[1]);
                case AddressingMode.Implied:
                    return instruction.Mnemonic;
                case AddressingMode.Indirect:
                    return string.Format("{0} (${1:X2}{2:X2})", instruction.Mnemonic, args[2], args[1]);
                case AddressingMode.XIndexedIndirect:
                    return string.Format("{0} (${1:X2},X)", instruction.Mnemonic, args[1]);
                case AddressingMode.IndirectYIndexed:
                    return string.Format("{0} (${1:X2}),Y", instruction.Mnemonic, args[1]);
                case AddressingMode.Relative:
                    return string.Format("{0} ${1:X2}", instruction.Mnemonic, args[1]);
                case AddressingMode.ZeroPage:
                    return string.Format("{0} ${1:X2}", instruction.Mnemonic, args[1]);
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