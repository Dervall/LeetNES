using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class RTS : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "RTS"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get { return new Dictionary<byte, AddressingMode> { { 0x60, AddressingMode.Implied }}; }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.Pc = 0;
            cpuState.Pc = cpuState.PopStack(memory);
            cpuState.Pc |= (ushort)(cpuState.PopStack(memory) << 8);
            cycles = 6;
        }
    }
}