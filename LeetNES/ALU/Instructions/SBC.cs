using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*
     SBC  Subtract Memory from Accumulator with Borrow

     A - M - C -> A                   N Z C I D V
                                      + + + - - +

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     immidiate     SBC #oper     E9    2     2
     zeropage      SBC oper      E5    2     3
     zeropage,X    SBC oper,X    F5    2     4
     absolute      SBC oper      ED    3     4
     absolute,X    SBC oper,X    FD    3     4*
     absolute,Y    SBC oper,Y    F9    3     4*
     (indirect,X)  SBC (oper,X)  E1    2     6
     (indirect),Y  SBC (oper),Y  F1    2     5*
     */
    public class SBC : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "SBC"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode> {
                    { 0xE9, AddressingMode.Immediate },
                    { 0xE5, AddressingMode.ZeroPage },
                    { 0xF5, AddressingMode.ZeroPageXIndexed },
                    { 0xED, AddressingMode.Absolute },
                    { 0xFD, AddressingMode.AbsoluteX },
                    { 0xF9, AddressingMode.AbsoluteY },
                    { 0xE1, AddressingMode.XIndexedIndirect },
                    { 0xF1, AddressingMode.IndirectYIndexed },
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {   
            //     A - M - C -> A                   N Z C I D V
            //                                      + + + - - +
            
            int a = cpuState.A;
            arg ^= 0xFF;
            a += arg;
            if (cpuState.IsFlagSet(CpuState.Flags.Carry))
            {
                ++a;
            }
            var byteResult = (byte) (a & 0xFF);
            cpuState.SetOverflow(cpuState.A, arg, byteResult);
            cpuState.A = byteResult;

            cpuState.SetNegativeFlag(cpuState.A);
            cpuState.SetZeroFlag(cpuState.A);
            cpuState.SetFlag(CpuState.Flags.Carry, (byteResult & 0x80) == 0);
        }
    }
}