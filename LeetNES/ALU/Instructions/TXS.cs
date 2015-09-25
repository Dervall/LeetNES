using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /// <summary>
    /// TXS  Transfer Index X to Stack Register
    ///
    /// X -> SP                          N Z C I D V
    ///                                  + + - - - -
    ///
    /// addressing    assembler    opc  bytes  cyles
    /// --------------------------------------------
    /// implied       TXS           9A    1     2
    /// </summary>
    public class TXS : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            { 0x9A, AddressingMode.Implied }
        };

        public override string Mnemonic
        {
            get { return "TXS"; }
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
            cpuState.Sp = cpuState.X;
        }
    }
}