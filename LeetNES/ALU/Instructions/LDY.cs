using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*LDY  Load Index Y with Memory

     M -> Y                           N Z C I D V
                                      + + - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     immidiate     LDY #oper     A0    2     2
     zeropage      LDY oper      A4    2     3
     zeropage,X    LDY oper,X    B4    2     4
     absolute      LDY oper      AC    3     4
     absolute,X    LDY oper,X    BC    3     4*
     */
    public class LDY : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode> {
            { 0xA0, AddressingMode.Immediate },
            { 0xA4, AddressingMode.ZeroPage },
            { 0xB4, AddressingMode.ZeroPageXIndexed },
            { 0xAC, AddressingMode.Absolute },
            { 0xBC, AddressingMode.AbsoluteX }                    
        };

        public override string Mnemonic
        {
            get { return "LDY"; }
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
            cpuState.Y = arg;
            cpuState.SetNegativeFlag(arg);
            cpuState.SetZeroFlag(arg);
        }
    }
}