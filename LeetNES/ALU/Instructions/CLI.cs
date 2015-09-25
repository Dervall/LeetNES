using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class CLI : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0x58, AddressingMode.Implied}
        };

        public override string Mnemonic { get { return "CLI"; } }

        public override IReadOnlyDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return addressingModes;
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.SetFlag(CpuState.Flags.InterruptDisable, false);
        }
    }
}