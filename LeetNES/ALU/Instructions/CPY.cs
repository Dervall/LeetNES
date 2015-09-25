using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class CPY : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0xC0, AddressingMode.Immediate},
            {0xC4, AddressingMode.ZeroPage},
            {0xCC, AddressingMode.Absolute},
        };

        public override string Mnemonic
        {
            get { return "CPY"; }
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
            var comparer = cpuState.Y;
            var cmp = comparer - arg;

            cpuState.SetFlag(CpuState.Flags.Carry, cmp >= 0);
            cpuState.SetNegativeFlag((byte)(cmp & 0xFF));
            cpuState.SetZeroFlag((byte)(cmp & 0xFF));
        }
    }
}