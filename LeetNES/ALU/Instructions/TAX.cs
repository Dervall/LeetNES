using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class TAX : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> { { 0xAA, AddressingMode.Implied } };

        public override string Mnemonic
        {
            get { return "TAX"; }
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
            cpuState.X = cpuState.A;
            cpuState.SetNegativeFlag(cpuState.X);
            cpuState.SetZeroFlag(cpuState.X);
        }
    }
}