using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*ROL  Rotate One Bit Left (Memory or Accumulator)

     C <- [76543210] <- C             N Z C I D V
                                      + + + - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     accumulator   ROL A         2A    1     2
     zeropage      ROL oper      26    2     5
     zeropage,X    ROL oper,X    36    2     6
     absolute      ROL oper      2E    3     6
     absolute,X    ROL oper,X    3E    3     7 */
    public class ROL : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "ROL"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    {0x2A, AddressingMode.Accumulator},
                    {0x26, AddressingMode.ZeroPage},
                    {0x36, AddressingMode.ZeroPageXIndexed},
                    {0x2E, AddressingMode.Absolute},
                    {0x3E, AddressingMode.AbsoluteX},
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            bool carry = cpuState.IsFlagSet(CpuState.Flags.Carry);
            cpuState.SetFlag(CpuState.Flags.Carry, arg & 0x80);
            arg <<= 1;
            if (carry)
            {
                arg |= 1;
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