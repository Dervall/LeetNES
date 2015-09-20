namespace LeetNES.ALU.Instructions
{
    public class BEQ : BaseBranchInstruction
    {
        public override string Mnemonic
        {
            get { return "BEQ"; }
        }

        protected override byte OpCode
        {
            get { return 0xF0; }
        }

        protected override bool ShouldBranch(CpuState cpuState, IMemory memory)
        {
            return cpuState.IsFlagSet(CpuState.Flags.Zero);
        }
    }

}