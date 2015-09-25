using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class PLP : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> {{0x28, AddressingMode.Implied}};

        public override string Mnemonic
        {
            get { return "PLP"; }
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
            var breakSet = cpuState.IsFlagSet(CpuState.Flags.Break);
            cpuState.StatusRegister = cpuState.PopStack(memory);
            cpuState.StatusRegister |= 1 << 5;
            cpuState.SetFlag(CpuState.Flags.Break, breakSet);
            cycles = 4;
        }
    }
}