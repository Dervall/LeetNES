using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class STX : BaseStoreInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0x86, AddressingMode.ZeroPage},
            {0x96, AddressingMode.ZeroPageYIndexed},
            {0x8E, AddressingMode.Absolute},
        };

        public override string Mnemonic
        {
            get { return "STX"; }
        }

        public override IReadOnlyDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return addressingModes;
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, Action<byte> write, ref int cycles)
        {
            write(cpuState.X);
        }
    }
}