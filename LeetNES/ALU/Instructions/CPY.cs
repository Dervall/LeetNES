using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class CPY : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "CPY"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    {0xC0, AddressingMode.Immediate},
                    {0xC4, AddressingMode.ZeroPage},
                    {0xCC, AddressingMode.Absolute},
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            var comparer = cpuState.Y;
            var cmp = comparer - arg;

            if (cmp < 0)
            {
                cpuState.SetFlag(CpuState.Flags.Carry, true);
            }
            cpuState.SetNegativeFlag((byte)(cmp & 0xFF));
            cpuState.SetZeroFlag((byte)(cmp & 0xFF));
        }
    }
}