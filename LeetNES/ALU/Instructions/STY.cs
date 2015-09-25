using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class STY : BaseStoreInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0x84, AddressingMode.ZeroPage},
            {0x94, AddressingMode.ZeroPageXIndexed},
            {0x8C, AddressingMode.Absolute},
        };

        public override string Mnemonic
        {
            get { return "STY"; }
        }

        public override IReadOnlyDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return addressingModes;
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory,  Action<byte> write, ref int cycles)
        {
            write(cpuState.Y);
        }
    }
}