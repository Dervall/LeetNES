using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class NOP : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> { { 0xEA, AddressingMode.Implied } };

        public override string Mnemonic
        {
            get { return "NOP"; }
        }

        public override IReadOnlyDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return addressingModes;
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            // NOP
        }
    }
}