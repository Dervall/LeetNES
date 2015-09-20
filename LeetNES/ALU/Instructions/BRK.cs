using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class BRK : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "BRK"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode> { { 0x00, AddressingMode.Implied } };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            throw new NotImplementedException();
        }
    }
}