using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*SEC  Set Carry Flag

     1 -> C                           N Z C I D V
                                      - - 1 - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     implied       SEC           38    1     2*/
    public class SEC : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "SEC"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode> {
                    {
                        0x38, AddressingMode.Implied
                    }};
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.SetFlag(CpuState.Flags.Carry, true);
        }
    }
}