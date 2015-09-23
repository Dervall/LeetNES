using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /*
    LSR  Shift One Bit Right (Memory or Accumulator)

     0 -> [76543210] -> C             N Z C I D V
                                      - + + - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     accumulator   LSR A         4A    1     2
     zeropage      LSR oper      46    2     5
     zeropage,X    LSR oper,X    56    2     6
     absolute      LSR oper      4E    3     6
     absolute,X    LSR oper,X    5E    3     7
    */
    public class LSR : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "LSR"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode> {
                    { 0x4A, AddressingMode.Accumulator },
                    { 0x46, AddressingMode.ZeroPage },
                    { 0x56, AddressingMode.ZeroPageXIndexed },
                    { 0x4E, AddressingMode.Absolute },
                    { 0x5E, AddressingMode.AbsoluteX }
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.SetFlag(CpuState.Flags.Carry, arg & 1);
            arg >>= 1;
            arg &= 0x7F;
            write(arg);
            cpuState.SetZeroFlag(arg);
            cpuState.SetNegativeFlag(0);

            switch (Variants[memory[cpuState.Pc]])
            {
                case AddressingMode.Accumulator:
                    cycles = 2;
                    break;
                case AddressingMode.ZeroPage:
                    cycles = 5;
                    break;
                case AddressingMode.ZeroPageXIndexed:
                    cycles = 6;
                    break;
                case AddressingMode.Absolute:
                    cycles = 6;
                    break;
                case AddressingMode.AbsoluteX: 
                    cycles = 7;
                    break;
            }
        }
    }
}