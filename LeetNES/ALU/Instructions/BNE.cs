namespace LeetNES.ALU.Instructions
{
    public class BNE : BaseBranchInstruction
    {
        public override string Mnemonic
        {
            get { return "BNE"; }
        }

        protected override byte OpCode
        {
            get { return 0xD0; }
        }

        protected override bool ShouldBranch(CpuState cpuState, IMemory memory)
        {
            return !cpuState.IsFlagSet(CpuState.Flags.Zero);
        }
    }
}