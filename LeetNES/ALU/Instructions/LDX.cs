using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /// <summary>
    /// LDX  Load Index X with Memory
    /// 
    /// M -> X                           N Z C I D V
    ///                                  + + - - - -
    /// 
    /// addressing    assembler    opc  bytes  cyles
    /// --------------------------------------------
    /// immidiate     LDX #oper     A2    2     2
    /// zeropage      LDX oper      A6    2     3
    /// zeropage,Y    LDX oper,Y    B6    2     4
    /// absolute      LDX oper      AE    3     4
    /// absolute,Y    LDX oper,Y    BE    3     4*
    /// </summary>
    public class LDX : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "LDX"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    { 0xA2, AddressingMode.Immediate },
                    { 0xA6, AddressingMode.ZeroPage },
                    { 0xB6, AddressingMode.ZeroPageYIndexed },
                    { 0xAE, AddressingMode.Absolute },
                    { 0xBE, AddressingMode.AbsoluteY },
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.X = arg;

            cpuState.SetNegativeFlag(arg);
            cpuState.SetZeroFlag(arg);
        }
    }
}