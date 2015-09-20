using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class CLC : BaseInstruction
    {
        public override string Mnemonic { get { return "CLC"; } }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    { 0x18, AddressingMode.Implied }    
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.SetFlag(CpuState.Flags.Carry, false);
        }
    }
}