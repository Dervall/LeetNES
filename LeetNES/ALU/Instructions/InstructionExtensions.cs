using System;

namespace LeetNES.ALU.Instructions
{
    public static class InstructionExtensions
    {
        public static string Disassemble(this IInstruction instruction, byte[] args)
        {
            switch (instruction.AddressingMode)
            {
                case AddressingMode.Accumulator:
                    return string.Format("{0} A", instruction.Mnemonic);
                case AddressingMode.Absolute:
                    return string.Format("{0} ${1:X}{2:X}", instruction.Mnemonic, args[1], args[0]);
                case AddressingMode.AbsoluteX:
                    return string.Format("{0} ${1:X}{2:X},X", instruction.Mnemonic, args[1], args[0]);
                case AddressingMode.AbsoluteY:
                    return string.Format("{0} ${1:X}{2:X},Y", instruction.Mnemonic, args[1], args[0]);
                case AddressingMode.Immediate:
                    return string.Format("{0} #${1:X}", instruction.Mnemonic, args[0]);
                case AddressingMode.Implied:
                    return instruction.Mnemonic;
                case AddressingMode.Indirect:
                    return string.Format("{0} (${1:X}{2:X})", instruction.Mnemonic, args[1], args[0]);
                case AddressingMode.XIndexedIndirect:
                    return string.Format("{0} (${1:X},X)", instruction.Mnemonic, args[0]);
                case AddressingMode.IndirectYIndexed:
                    return string.Format("{0} (${1:X}),Y", instruction.Mnemonic, args[0]);
                case AddressingMode.Relative:
                    return string.Format("{0} ${1:X}", instruction.Mnemonic, args[0]);
                case AddressingMode.ZeroPage:
                    return string.Format("{0} ${1:X}", instruction.Mnemonic, args[0]);
                case AddressingMode.ZeroPageXIndexed:
                    return string.Format("{0} ${1:X},X", instruction.Mnemonic, args[0]);
                case AddressingMode.ZeroPageYIndexed:
                    return string.Format("{0} ${1:X},Y", instruction.Mnemonic, args[0]);
                default:
                    throw new Exception("Disassembly for addressing mode is unimplemented");
            }
        }
    }
}