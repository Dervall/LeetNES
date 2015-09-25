using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
   /* TAY  Transfer Accumulator to Index Y

     A -> Y                           N Z C I D V
                                      + + - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     implied       TAY           A8    1     2 */
    public class TAY : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> {{0xA8, AddressingMode.Implied}};

        public override string Mnemonic
        {
            get { return "TAY"; }
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
            cpuState.Y = cpuState.A;
            cpuState.SetNegativeFlag(cpuState.Y);
            cpuState.SetZeroFlag(cpuState.Y);
        }
    }
}