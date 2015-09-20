using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class DEX : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "DEX"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    {0xCA, AddressingMode.Implied},
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            var res = (byte)((cpuState.X - 1) & 0xFF);
            cpuState.X = res;
            cpuState.SetNegativeFlag(res);
            cpuState.SetZeroFlag(res);

        }
    }
}