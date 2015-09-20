using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class DEY : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "DEY"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    {0x88, AddressingMode.Implied},
                };
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