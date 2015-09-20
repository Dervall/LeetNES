using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
   /* TAY  Transfer Accumulator to Index Y

     A -> Y                           N Z C I D V
                                      + + - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     implied       TAY           A8    1     2 */
    public class TAY : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "TAY"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode> { { 0xA8, AddressingMode.Implied } };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.Y = cpuState.A;
            cpuState.SetNegativeFlag(cpuState.Y);
            cpuState.SetZeroFlag(cpuState.Y);
        }
    }
}