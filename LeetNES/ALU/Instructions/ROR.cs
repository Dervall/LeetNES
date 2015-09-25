using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*ROR  Rotate One Bit Right (Memory or Accumulator)

     C -> [76543210] -> C             N Z C I D V
                                      + + + - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     accumulator   ROR A         6A    1     2
     zeropage      ROR oper      66    2     5
     zeropage,X    ROR oper,X    76    2     6
     absolute      ROR oper      6E    3     6
     absolute,X    ROR oper,X    7E    3     7
     */
    public class ROR : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0x6A, AddressingMode.Accumulator},
            {0x66, AddressingMode.ZeroPage},
            {0x76, AddressingMode.ZeroPageXIndexed},
            {0x6E, AddressingMode.Absolute},
            {0x7E, AddressingMode.AbsoluteX},
        };

        public override string Mnemonic
        {
            get { return "ROR"; }
        }

        public override IReadOnlyDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return addressingModes;
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            bool carry = cpuState.IsFlagSet(CpuState.Flags.Carry);
            cpuState.SetFlag(CpuState.Flags.Carry, arg & 0x1);
            arg >>= 1;
            if (carry)
            {
                arg |= 0x80;
            }
            write(arg);
            cpuState.SetNegativeFlag(arg);
            cpuState.SetZeroFlag(arg);

            switch (Variants[memory[cpuState.Pc]])
            {
                case AddressingMode.Accumulator:
                    cycles = 2;
                    break;
                case AddressingMode.ZeroPage:
                    cycles = 5;
                    break;
                case AddressingMode.ZeroPageXIndexed:
                    cycles = 6;
                    break;
                case AddressingMode.Absolute:
                    cycles = 6;
                    break;
                case AddressingMode.AbsoluteX:
                    cycles = 7;
                    break;
            }
        }
    }
}