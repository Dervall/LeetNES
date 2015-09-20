using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class CLI : BaseInstruction
    {
        public override string Mnemonic { get { return "CLI"; } }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    { 0x58, AddressingMode.Implied }    
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.SetFlag(CpuState.Flags.InterruptDisable, false);
        }
    }
}