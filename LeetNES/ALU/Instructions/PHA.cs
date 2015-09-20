using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*PHA  Push Accumulator on Stack

     push A                           N Z C I D V
                                      - - - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     implied       PHA           48    1     3*/
    public class PHA : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "PHA"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get { return new Dictionary<byte, AddressingMode> { { 0x48, AddressingMode.Implied }}; }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.PushStack(cpuState.A, memory);
            cycles = 3;
        }
    }
}