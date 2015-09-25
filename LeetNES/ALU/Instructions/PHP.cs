using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class PHP : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> { { 0x08, AddressingMode.Implied } };

        public override string Mnemonic
        {
            get { return "PHP"; }
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
            // Break flag is always set
            cpuState.PushStack((byte) (cpuState.StatusRegister | (byte)CpuState.Flags.Break), memory);
            cycles = 3;
        }
    }
}