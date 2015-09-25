using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*RTI  Return from Interrupt

     pull SR, pull PC                 N Z C I D V
                                      from stack

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     implied       RTI           40    1     6
     */
    public class RTI : IInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> {{0x40, AddressingMode.Implied}};

        public string Mnemonic
        {
            get { return "RTI"; }
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
            var breakSet = cpuState.IsFlagSet(CpuState.Flags.Break);
            cpuState.StatusRegister = cpuState.PopStack(memory);
            cpuState.SetFlag(CpuState.Flags.Break, breakSet);
            cpuState.StatusRegister |= 1 << 5;

            var low = cpuState.PopStack(memory);
            var high = cpuState.PopStack(memory);
            
            cpuState.Pc = (ushort)((high << 8) | low);

            return 6;
        }
    }
}