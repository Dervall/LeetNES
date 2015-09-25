using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /// <summary>
    /// LDA  Load Accumulator with Memory
    ///
    /// M -> A                           N Z C I D V
    ///                                  + + - - - -
    ///
    /// addressing    assembler    opc  bytes  cyles
    /// --------------------------------------------
    /// immidiate     LDA #oper     A9    2     2
    /// zeropage      LDA oper      A5    2     3
    /// zeropage,X    LDA oper,X    B5    2     4
    /// absolute      LDA oper      AD    3     4
    /// absolute,X    LDA oper,X    BD    3     4*
    /// absolute,Y    LDA oper,Y    B9    3     4*
    /// (indirect,X)  LDA (oper,X)  A1    2     6
    /// (indirect),Y  LDA (oper),Y  B1    2     5*
    /// </summary>
    public class LDA : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0xA9, AddressingMode.Immediate},
            {0xA5, AddressingMode.ZeroPage},
            {0xB5, AddressingMode.ZeroPageXIndexed},
            {0xAD, AddressingMode.Absolute},
            {0xBD, AddressingMode.AbsoluteX},
            {0xB9, AddressingMode.AbsoluteY},
            {0xA1, AddressingMode.XIndexedIndirect},
            {0xB1, AddressingMode.IndirectYIndexed},
        };

        public override string Mnemonic
        {
            get { return "LDA"; }
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
            cpuState.A = arg;
            cpuState.SetNegativeFlag(arg);
            cpuState.SetZeroFlag(arg);            
        }
    }
}