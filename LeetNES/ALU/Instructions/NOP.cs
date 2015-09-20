using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class NOP : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "NOP"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get { return new Dictionary<byte, AddressingMode>{{0xEA, AddressingMode.Implied}}; }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            // NOP
        }
    }
}