using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*CPX  Compare Memory and Index X

     X - M                            N Z C I D V
                                      + + + - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     immidiate     CPX #oper     E0    2     2
     zeropage      CPX oper      E4    2     3
     absolute      CPX oper      EC    3     4*/
    public class CPX : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "CPX"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    {0xE0, AddressingMode.Immediate},
                    {0xE4, AddressingMode.ZeroPage},
                    {0xEC, AddressingMode.Absolute},
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            var comparer = cpuState.X;
            var cmp = comparer - arg;

            cpuState.SetFlag(CpuState.Flags.Carry, cmp >= 0); 
            cpuState.SetNegativeFlag((byte) (cmp & 0xFF));
            cpuState.SetZeroFlag((byte) (cmp & 0xFF));
        }
    }
}