using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class INY : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {
                0xC8, AddressingMode.Implied
            }
        };

        public override string Mnemonic
        {
            get { return "INY"; }
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
            cpuState.Y++;
            cpuState.SetZeroFlag(cpuState.Y);
            cpuState.SetNegativeFlag(cpuState.Y);
        }
    }
}