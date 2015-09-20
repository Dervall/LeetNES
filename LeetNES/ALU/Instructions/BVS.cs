namespace LeetNES.ALU.Instructions
{
    public class BVS : BaseBranchInstruction
    {
        public override string Mnemonic
        {
            get { return "BVS"; }
        }

        protected override byte OpCode
        {
            get { return 0x70; }
        }

        protected override bool ShouldBranch(CpuState cpuState, IMemory memory)
        {
            return cpuState.IsFlagSet(CpuState.Flags.Overflow);
        }
    }
}