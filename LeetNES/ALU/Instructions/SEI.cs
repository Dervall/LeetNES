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
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {
                0x78, AddressingMode.Implied
            }
        };

        public override string Mnemonic
        {
            get { return "SEI"; }
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
            cpuState.SetFlag(CpuState.Flags.InterruptDisable, true);
        }
    }
}