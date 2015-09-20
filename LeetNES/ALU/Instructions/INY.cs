using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class INY : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "INY"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode> {
                    {
                        0xC8, AddressingMode.Implied
                    }};
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.Y++;
            cpuState.SetZeroFlag(cpuState.Y);
            cpuState.SetNegativeFlag(cpuState.Y);
        }
    }
}