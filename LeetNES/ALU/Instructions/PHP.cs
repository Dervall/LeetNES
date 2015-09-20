using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class PHP : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "PHA"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get { return new Dictionary<byte, AddressingMode> { { 0x08, AddressingMode.Implied }}; }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.PushStack(cpuState.StatusRegister, memory);
            cycles = 3;
        }
    }
}