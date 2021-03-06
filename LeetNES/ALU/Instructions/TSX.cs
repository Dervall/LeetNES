using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*
     TSX  Transfer Stack Pointer to Index X

     SP -> X                          N Z C I D V
                                      + + - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     implied       TSX           BA    1     2
     */
    public class TSX : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> { { 0xBA, AddressingMode.Implied } };

        public override string Mnemonic
        {
            get { return "TSX"; }
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
            cpuState.X = cpuState.Sp;
            cpuState.SetNegativeFlag(cpuState.X);
            cpuState.SetZeroFlag(cpuState.X);
        }
    }
}