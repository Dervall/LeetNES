using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    // ASL  Shift Left One Bit (Memory or Accumulator)
    // 
    // C <- [76543210] <- 0             N Z C I D V
    //                                  + + + - - -
    // 
    // addressing    assembler    opc  bytes  cyles
    // --------------------------------------------
    // accumulator   ASL A         0A    1     2
    // zeropage      ASL oper      06    2     5
    // zeropage,X    ASL oper,X    16    2     6
    // absolute      ASL oper      0E    3     6
    // absolute,X    ASL oper,X    1E    3     7


    /// AND  AND Memory with Accumulator
    /// 
    /// A AND M -> A                     N Z C I D V
    ///                                  + + - - - -
    /// 
    /// addressing    assembler    opc  bytes  cyles
    /// --------------------------------------------
    /// immidiate     AND #oper     29    2     2
    /// zeropage      AND oper      25    2     3
    /// zeropage,X    AND oper,X    35    2     4
    /// absolute      AND oper      2D    3     4
    /// absolute,X    AND oper,X    3D    3     4*
    /// absolute,Y    AND oper,Y    39    3     4*
    /// (indirect,X)  AND (oper,X)  21    2     6
    /// (indirect),Y  AND (oper),Y  31    2     5*
    public class AND : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "AND"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    {0x29, AddressingMode.Immediate},
                    {0x25, AddressingMode.ZeroPage},
                    {0x35, AddressingMode.ZeroPageXIndexed},
                    {0x2D, AddressingMode.Absolute},
                    {0x3D, AddressingMode.AbsoluteX},
                    {0x39, AddressingMode.AbsoluteY},
                    {0x21, AddressingMode.XIndexedIndirect},
                    {0x31, AddressingMode.IndirectYIndexed}
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.A &= arg;
            cpuState.SetNegativeFlag(cpuState.A);
            cpuState.SetZeroFlag(cpuState.A);
        }
    }
}