using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*EOR  Exclusive-OR Memory with Accumulator

     A EOR M -> A                     N Z C I D V
                                      + + - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     immidiate     EOR #oper     49    2     2
     zeropage      EOR oper      45    2     3
     zeropage,X    EOR oper,X    55    2     4
     absolute      EOR oper      4D    3     4
     absolute,X    EOR oper,X    5D    3     4*
     absolute,Y    EOR oper,Y    59    3     4*
     (indirect,X)  EOR (oper,X)  41    2     6
     (indirect),Y  EOR (oper),Y  51    2     5*
     */
    public class EOR : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0x49, AddressingMode.Immediate},
            {0x45, AddressingMode.ZeroPage},
            {0x55, AddressingMode.ZeroPageXIndexed},
            {0x4D, AddressingMode.Absolute},
            {0x5D, AddressingMode.AbsoluteX},
            {0x59, AddressingMode.AbsoluteY},
            {0x41, AddressingMode.XIndexedIndirect},
            {0x51, AddressingMode.IndirectYIndexed},
        };

        public override string Mnemonic
        {
            get { return "EOR"; }
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
            cpuState.A ^= arg;
            cpuState.SetNegativeFlag(cpuState.A);
            cpuState.SetZeroFlag(cpuState.A);
        }
    }
}