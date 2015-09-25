using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    /* INX  Increment Index X by One

     X + 1 -> X                       N Z C I D V
                                      + + - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     implied       INX           E8    1     2 */
    public class INX : BaseInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {
                0xE8, AddressingMode.Implied
            }
        };

        public override string Mnemonic
        {
            get { return "INX"; }
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
            cpuState.X++;
            cpuState.SetZeroFlag(cpuState.X);
            cpuState.SetNegativeFlag(cpuState.X);
        }
    }
}