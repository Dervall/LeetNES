using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*INC  Increment Memory by One

     M + 1 -> M                       N Z C I D V
                                      + + - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     zeropage      INC oper      E6    2     5
     zeropage,X    INC oper,X    F6    2     6
     absolute      INC oper      EE    3     6
     absolute,X    INC oper,X    FE    3     7*/
    public class INC : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "INC"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    {0xE6, AddressingMode.ZeroPage},
                    {0xF6, AddressingMode.ZeroPageXIndexed},
                    {0xEE, AddressingMode.Absolute},
                    {0xFE, AddressingMode.AbsoluteX},
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            ++arg;
            write(arg);
            cpuState.SetNegativeFlag(arg);
            cpuState.SetZeroFlag(arg);

            switch (Variants[memory[cpuState.Pc]])
            {
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