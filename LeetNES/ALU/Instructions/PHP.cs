using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class PHP : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "PHP"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get { return new Dictionary<byte, AddressingMode> { { 0x08, AddressingMode.Implied }}; }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            // Break flag is always set
            cpuState.PushStack((byte) (cpuState.StatusRegister | (byte)CpuState.Flags.Break), memory);
            cycles = 3;
        }
    }
}