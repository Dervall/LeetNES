using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /// <summary>
    /// CLD  Clear Decimal Mode
    /// 
    /// 0 -> D                           N Z C I D V
    ///                                  - - - - 0 -
    /// 
    /// addressing    assembler    opc  bytes  cyles
    /// --------------------------------------------
    /// implied       CLD           D8    1     2
    /// </summary>
    public class CLD : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0xD8, AddressingMode.Implied}
        };

        public override string Mnemonic { get { return "CLD"; } }

        public override IReadOnlyDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return addressingModes;
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            cpuState.SetFlag(CpuState.Flags.DecimalMode, false);
        }
    }
}