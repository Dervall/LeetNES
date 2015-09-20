using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class STY : BaseStoreInstruction
    {
        public override string Mnemonic
        {
            get { return "STY"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    {0x84, AddressingMode.ZeroPage},
                    {0x94, AddressingMode.ZeroPageXIndexed},
                    {0x8C, AddressingMode.Absolute},
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory,  Action<byte> write, ref int cycles)
        {
            write(cpuState.Y);
        }
    }
}