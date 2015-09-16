using System.Collections;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public interface IInstruction
    {        
        string Mnemonic { get; }
        IDictionary<byte, AddressingMode> Variants { get; }

        int Execute(Cpu.State cpuState, IMemory memory);
    }
}