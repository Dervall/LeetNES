using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*RTI  Return from Interrupt

     pull SR, pull PC                 N Z C I D V
                                      from stack

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     implied       RTI           40    1     6
     */
    public class RTI : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "RTI"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get { return new Dictionary<byte, AddressingMode>{{0x40, AddressingMode.Implied}}; }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.StatusRegister = cpuState.PopStack(memory);
            cpuState.Pc = 0;
            cpuState.Pc = cpuState.PopStack(memory);
            cpuState.Pc |= (ushort)(cpuState.PopStack(memory) << 8);
            cycles = 6;
        }
    }
}