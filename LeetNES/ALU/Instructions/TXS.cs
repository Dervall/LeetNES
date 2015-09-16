using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class TXS : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "TXS"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get 
            { 
                return new Dictionary<byte, AddressingMode>
                {
                    { 0x9A, AddressingMode.Implied }
                }; 
            }
        }

        protected override void InternalExecute(Cpu.State cpuState, IMemory memory, byte arg, ref int cycles)
        {
            cpuState.Sp = cpuState.X;
            cpuState.SetFlag(Cpu.Flags.Negative, 0x7000 & cpuState.Sp);
            cpuState.SetFlag(Cpu.Flags.Zero, cpuState.Sp == 0);
        }
    }
}