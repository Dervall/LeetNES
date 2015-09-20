using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    ///ADC  Add Memory to Accumulator with Carry
    ///
    ///A + M + C -> A, C                N Z C I D V
    ///                                + + + - - +
    ///
    ///addressing    assembler    opc  bytes  cyles
    ///--------------------------------------------
    ///immidiate     ADC #oper     69    2     2
    ///zeropage      ADC oper      65    2     3
    ///zeropage,X    ADC oper,X    75    2     4
    ///absolute      ADC oper      6D    3     4
    ///absolute,X    ADC oper,X    7D    3     4*
    ///absolute,Y    ADC oper,Y    79    3     4*
    ///(indirect,X)  ADC (oper,X)  61    2     6
    ///(indirect),Y  ADC (oper),Y  71    2     5*
    public class ADC : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "ADC"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get 
            { 
                return new Dictionary<byte, AddressingMode>
                {
                    {0x69, AddressingMode.Immediate},
                    {0x65, AddressingMode.ZeroPage},
                    {0x75, AddressingMode.ZeroPageXIndexed},
                    {0x6D, AddressingMode.Absolute},
                    {0x7D, AddressingMode.AbsoluteX},
                    {0x79, AddressingMode.AbsoluteY},
                    {0x61, AddressingMode.XIndexedIndirect},
                    {0x71, AddressingMode.IndirectYIndexed}
                }; 
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            var sum = (ushort) (arg + cpuState.A);
            cpuState.A = (byte) (sum & 0xFF);
            cpuState.SetZeroFlag(cpuState.A);
            cpuState.SetNegativeFlag(cpuState.A);
            cpuState.SetFlag(CpuState.Flags.Carry, sum & 0xFF00);
            cpuState.SetFlag(CpuState.Flags.Overflow, sum & 0xFF00); // ??
        }
    }
}
