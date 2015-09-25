using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*PLA  Pull Accumulator from Stack

     pull A                           N Z C I D V
                                      + + - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     implied       PLA           68    1     4*/
    public class PLA : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> { { 0x68, AddressingMode.Implied } };

        public override string Mnemonic
        {
            get { return "PLA"; }
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
            cpuState.A = cpuState.PopStack(memory);
            cpuState.SetNegativeFlag(cpuState.A);
            cpuState.SetZeroFlag(cpuState.A);
            cycles = 4;
        }
    }
}