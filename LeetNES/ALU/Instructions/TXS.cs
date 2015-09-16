namespace LeetNES.ALU.Instructions
{
    public class TXS : ImpliedInstruction
    {
        public override byte OpCode
        {
            get { return 0x9A; }
        }

        public override string Mnemonic
        {
            get { return "TXS"; }
        }

        public override int Execute(Cpu.State cpuState, IMemory memory)
        {
            cpuState.s = cpuState.x;
            cpuState.SetFlag(Cpu.Flags.Negative, 0x7000 & cpuState.s);
            cpuState.SetFlag(Cpu.Flags.Zero, cpuState.s == 0);
            cpuState.p += 1;
            return 2;
        }
    }
}