using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class BRK : IInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> {{0x00, AddressingMode.Implied}};

        public string Mnemonic
        {
            get { return "BRK"; }
        }

        public IReadOnlyDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return addressingModes;
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