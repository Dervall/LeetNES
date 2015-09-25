using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class TXA : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> { { 0x8A, AddressingMode.Implied } };

        public override string Mnemonic
        {
            get { return "TXA"; }
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
            cpuState.A = cpuState.X;
            cpuState.SetNegativeFlag(cpuState.A);
            cpuState.SetZeroFlag(cpuState.A);
        }
    }
}