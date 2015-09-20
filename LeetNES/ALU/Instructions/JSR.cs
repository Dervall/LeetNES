using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*JSR  Jump to New Location Saving Return Address

     push (PC+2),                     N Z C I D V
     (PC+1) -> PCL                    - - - - - -
     (PC+2) -> PCH

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     absolute      JSR oper      20    3     6*/
    public class JSR : IInstruction
    {
        public string Mnemonic
        {
            get { return "JSR"; }
        }

        public IDictionary<byte, AddressingMode> Variants
        {
            get { return new Dictionary<byte, AddressingMode> {{ 0x20, AddressingMode.Absolute}}; }
        }

        public int Execute(CpuState cpuState, IMemory memory)
        {
            var ret = cpuState.Pc + 3;
            cpuState.PushStack((byte) (ret & 0xFF), memory);
            cpuState.PushStack((byte) ((ret & 0xFF00) >> 8), memory);
            cpuState.Pc = memory.ReadShort(cpuState.Pc + 1);
            return 6;
        }
    }
}