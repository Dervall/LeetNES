using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class PLP : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "PLP"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get { return new Dictionary<byte, AddressingMode> { { 0x28, AddressingMode.Implied }}; }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.StatusRegister = cpuState.PopStack(memory);
            cycles = 4;
        }
    }
}