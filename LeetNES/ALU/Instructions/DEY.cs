using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class DEY : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0x88, AddressingMode.Implied},
        };

        public override string Mnemonic
        {
            get { return "DEY"; }
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
            var res = (byte)((cpuState.Y - 1) & 0xFF);
            cpuState.Y = res;
            cpuState.SetNegativeFlag(res);
            cpuState.SetZeroFlag(res);

        }
    }
}