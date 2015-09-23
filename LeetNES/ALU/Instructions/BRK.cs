using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class BRK : IInstruction
    {
        public string Mnemonic
        {
            get { return "BRK"; }
        }

        public IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode> { { 0x00, AddressingMode.Implied } };
            }
        }

        public int Execute(CpuState cpuState, IMemory memory)
        {
            throw new Exception("die");
            
            
            
            cpuState.Interrupt(0xFFFE, memory);
            return 7;
        }
    }
}