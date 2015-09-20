using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*TYA  Transfer Index Y to Accumulator

     Y -> A                           N Z C I D V
                                      + + - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     implied       TYA           98    1     2
     */
    public class TYA : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "TYA"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode> { { 0x98, AddressingMode.Implied } };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.A = cpuState.Y;
            cpuState.SetNegativeFlag(cpuState.A);
            cpuState.SetZeroFlag(cpuState.A);
        }
    }
}