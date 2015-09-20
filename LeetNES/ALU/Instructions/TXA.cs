using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class TXA : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "TXA"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode> { { 0x8A, AddressingMode.Implied } };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.A = cpuState.X;
            cpuState.SetNegativeFlag(cpuState.A);
            cpuState.SetZeroFlag(cpuState.A);
        }
    }
}