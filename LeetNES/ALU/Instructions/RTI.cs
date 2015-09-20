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
    public class RTI : IInstruction
    {
        public string Mnemonic
        {
            get { return "RTI"; }
        }

        public IDictionary<byte, AddressingMode> Variants
        {
            get { return new Dictionary<byte, AddressingMode>{{0x40, AddressingMode.Implied}}; }
        }

        public int Execute(CpuState cpuState, IMemory memory)
        {
            cpuState.StatusRegister = cpuState.PopStack(memory);
            var high = cpuState.PopStack(memory);
            var low = cpuState.PopStack(memory);
            cpuState.Pc = (ushort)((high << 8) | low);

            return 6;
        }
    }
}