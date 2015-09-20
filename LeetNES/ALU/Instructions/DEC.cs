using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*
     DEC  Decrement Memory by One

     M - 1 -> M                       N Z C I D V
                                      + + - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     zeropage      DEC oper      C6    2     5
     zeropage,X    DEC oper,X    D6    2     6
     absolute      DEC oper      CE    3     3
     absolute,X    DEC oper,X    DE    3     7
     */
    public class DEC : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "DEC"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    {0xC6, AddressingMode.ZeroPage},
                    {0xD6, AddressingMode.ZeroPageXIndexed},
                    {0xCE, AddressingMode.Absolute},
                    {0xDE, AddressingMode.AbsoluteX},
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            var res = (byte) ((arg - 1)*0xFF);
            write(res);
            cpuState.SetNegativeFlag(res);
            cpuState.SetZeroFlag(res);

            switch (Variants[memory[cpuState.Pc]])
            {
                case AddressingMode.ZeroPage:
                    cycles = 5;
                    break;
                case AddressingMode.ZeroPageXIndexed:
                    cycles = 6;
                    break;
                case AddressingMode.Absolute:
                    cycles = 3;
                    break;
                case AddressingMode.AbsoluteX:
                    cycles = 7;
                    break;
            }
        }
    }
}