using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class BIT : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0x24, AddressingMode.ZeroPage},
            {0x2C, AddressingMode.Absolute},
        };

        public override string Mnemonic
        {
            get { return "BIT"; }
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
            cpuState.SetZeroFlag((byte) (arg & cpuState.A));
            cpuState.SetFlag(CpuState.Flags.Negative, arg & 0x80);
            cpuState.SetFlag(CpuState.Flags.Overflow, arg & 0x40);
        }
    }
}