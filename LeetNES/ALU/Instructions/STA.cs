using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*STA  Store Accumulator in Memory

     A -> M                           N Z C I D V
                                      - - - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     zeropage      STA oper      85    2     3
     zeropage,X    STA oper,X    95    2     4
     absolute      STA oper      8D    3     4
     absolute,X    STA oper,X    9D    3     5
     absolute,Y    STA oper,Y    99    3     5
     (indirect,X)  STA (oper,X)  81    2     6
     (indirect),Y  STA (oper),Y  91    2     6
     */
    public class STA : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "STA"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode> {
                    { 0x85, AddressingMode.ZeroPage },
                    { 0x95, AddressingMode.ZeroPageXIndexed },
                    { 0x8D, AddressingMode.Absolute },
                    { 0x9D, AddressingMode.AbsoluteX },
                    { 0x99, AddressingMode.AbsoluteY },
                    { 0x81, AddressingMode.XIndexedIndirect },
                    { 0x91, AddressingMode.IndirectYIndexed },
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            write(cpuState.A);

            switch (Variants[memory[cpuState.Pc]])
            {
                case AddressingMode.AbsoluteX:
                case AddressingMode.AbsoluteY:
                    cycles = 5;
                    break;
                case AddressingMode.XIndexedIndirect:
                case AddressingMode.IndirectYIndexed:
                    cycles = 6;
                    break;
            }
        }
    }
}