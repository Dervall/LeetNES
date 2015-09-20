using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class TAX : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "TAX"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>{{0xAA, AddressingMode.Implied}};
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.X = cpuState.A;
            cpuState.SetNegativeFlag(cpuState.X);
            cpuState.SetZeroFlag(cpuState.X);
        }
    }
}