namespace LeetNES.ALU.Instructions
{
    public class SEI : ImpliedInstruction
    {
        public override byte OpCode
        {
            get { return 0x78; }
        }

        public override string Mnemonic
        {
            get { return "SEI"; }
        }

        public override int Execute(Cpu.State cpuState, IMemory memory)
        {
            cpuState.SetFlag(Cpu.Flags.InterruptDisable, true);
            cpuState.p++;
            return 2;
        }
    }
}