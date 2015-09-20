using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /// <summary>
    ///  SEI  Set Interrupt Disable Status
    ///
    /// 1 -> I                           N Z C I D V
    ///                                  - - - 1 - -
    ///
    /// addressing    assembler    opc  bytes  cyles
    /// --------------------------------------------
    /// implied       SEI           78    1     2
    /// </summary>
    public class SEI : BaseInstruction
    {        
        public override string Mnemonic
        {
            get { return "SEI"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get 
            { 
                return new Dictionary<byte, AddressingMode> {
                {
                    0x78, AddressingMode.Implied
                }}; 
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.SetFlag(CpuState.Flags.InterruptDisable, true);
        }
    }

    /*STX  Store Index X in Memory

     X -> M                           N Z C I D V
                                      - - - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     zeropage      STX oper      86    2     3
     zeropage,Y    STX oper,Y    96    2     4
     absolute      STX oper      8E    3     4*/
    public class STX : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "STX"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    {0x86, AddressingMode.ZeroPage},
                    {0x96, AddressingMode.ZeroPageYIndexed},
                    {0x8E, AddressingMode.Absolute},
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            write(cpuState.X);
        }
    }

}