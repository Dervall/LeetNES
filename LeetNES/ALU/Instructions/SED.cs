using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class SED : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "SED"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode> {
                    {
                        0xF8, AddressingMode.Implied
                    }};
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.SetFlag(CpuState.Flags.DecimalMode, true);
        }
    }
}