using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class RTS : IInstruction
    {
        public string Mnemonic
        {
            get { return "RTS"; }
        }

        public IDictionary<byte, AddressingMode> Variants
        {
            get { return new Dictionary<byte, AddressingMode> { { 0x60, AddressingMode.Implied }}; }
        }

        public int Execute(CpuState cpuState, IMemory memory)
        {
            var high = cpuState.PopStack(memory);
            var low = cpuState.PopStack(memory);
            cpuState.Pc = (ushort)((high << 8) | low);

            return 6;
        }
    }
}