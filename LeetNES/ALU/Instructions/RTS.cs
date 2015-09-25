using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class RTS : IInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> {{0x60, AddressingMode.Implied}};

        public string Mnemonic
        {
            get { return "RTS"; }
        }

        public IReadOnlyDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return addressingModes;
            }
        }

        public int Execute(CpuState cpuState, IMemory memory)
        {
            var low = cpuState.PopStack(memory);
            var high = cpuState.PopStack(memory);
            
            cpuState.Pc = (ushort)((high << 8) | low);
            ++cpuState.Pc;
            return 6;
        }
    }
}