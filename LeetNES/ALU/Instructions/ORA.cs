using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*ORA  OR Memory with Accumulator

     A OR M -> A                      N Z C I D V
                                      + + - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     immidiate     ORA #oper     09    2     2
     zeropage      ORA oper      05    2     3
     zeropage,X    ORA oper,X    15    2     4
     absolute      ORA oper      0D    3     4
     absolute,X    ORA oper,X    1D    3     4*
     absolute,Y    ORA oper,Y    19    3     4*
     (indirect,X)  ORA (oper,X)  01    2     6
     (indirect),Y  ORA (oper),Y  11    2     5*
     */
    public class ORA : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0x09, AddressingMode.Immediate},
            {0x05, AddressingMode.ZeroPage},
            {0x15, AddressingMode.ZeroPageXIndexed},
            {0x0D, AddressingMode.Absolute},
            {0x1D, AddressingMode.AbsoluteX},
            {0x19, AddressingMode.AbsoluteY},
            {0x01, AddressingMode.XIndexedIndirect},
            {0x11, AddressingMode.IndirectYIndexed},
        };

        public override string Mnemonic
        {
            get { return "ORA"; }
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
            cpuState.A |= arg;
            cpuState.SetNegativeFlag(cpuState.A);
            cpuState.SetZeroFlag(cpuState.A);
        }
    }
}