using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*CMP  Compare Memory with Accumulator

     A - M                            N Z C I D V
                                      + + + - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     immidiate     CMP #oper     C9    2     2
     zeropage      CMP oper      C5    2     3
     zeropage,X    CMP oper,X    D5    2     4
     absolute      CMP oper      CD    3     4
     absolute,X    CMP oper,X    DD    3     4*
     absolute,Y    CMP oper,Y    D9    3     4*
     (indirect,X)  CMP (oper,X)  C1    2     6
     (indirect),Y  CMP (oper),Y  D1    2     5*
     */

    public class CMP : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "CMP"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    {0xC9, AddressingMode.Immediate},
                    {0xC5, AddressingMode.ZeroPage},
                    {0xD5, AddressingMode.ZeroPageXIndexed},
                    {0xCD, AddressingMode.Absolute},
                    {0xDD, AddressingMode.AbsoluteX},
                    {0xD9, AddressingMode.AbsoluteY},
                    {0xC1, AddressingMode.XIndexedIndirect},
                    {0xD1, AddressingMode.IndirectYIndexed},
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            var comparer = cpuState.A;
            var cmp = comparer - arg;

            if (cmp < 0)
            {
                cpuState.SetFlag(CpuState.Flags.Carry, true);
            }
            cpuState.SetNegativeFlag((byte) (cmp & 0xFF));
            cpuState.SetZeroFlag((byte) (cmp & 0xFF));
        }

    }
}