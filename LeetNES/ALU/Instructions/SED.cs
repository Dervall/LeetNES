using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class SED : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {
                0xF8, AddressingMode.Implied
            }
        };

        public override string Mnemonic
        {
            get { return "SED"; }
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
            cpuState.SetFlag(CpuState.Flags.DecimalMode, true);
        }
    }
}