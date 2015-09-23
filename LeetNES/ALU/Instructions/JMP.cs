using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /// <summary>
    /// JMP  Jump to New Location
    ///
    /// (PC+1) -> PCL                    N Z C I D V
    /// (PC+2) -> PCH                    - - - - - -
    ///
    /// addressing    assembler    opc  bytes  cyles
    /// --------------------------------------------
    /// absolute      JMP oper      4C    3     3
    /// indirect      JMP (oper)    6C    3     5
    /// </summary>
    public class JMP : IInstruction
    {
        public string Mnemonic { get { return "JMP"; } }

        public IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    { 0x4C, AddressingMode.Absolute },
                    { 0x6C, AddressingMode.Indirect }
                };
            }
        }

        public int Execute(CpuState cpuState, IMemory memory)
        {
            int cycles = 3;
            ushort dest = memory.ReadShort(cpuState.Pc + 1);
            if (Variants[memory[cpuState.Pc]] == AddressingMode.Indirect)
            {
                // Reading the address cannot page wrap
                var secondByte = (dest + 1) & 0xFF;
                secondByte |= dest & 0xFF00;
                dest = (ushort) (memory[dest] | (memory[secondByte] << 8));
                cycles = 5;
            }
            cpuState.Pc = dest;
            return cycles;
        }
    }
}