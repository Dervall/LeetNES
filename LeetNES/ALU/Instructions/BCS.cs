namespace LeetNES.ALU.Instructions
{
    public class BCS : BaseBranchInstruction
    {
        public override string Mnemonic
        {
            get { return "BCS"; }
        }

        protected override byte OpCode
        {
            get { return 0xB0; }
        }

        protected override bool ShouldBranch(CpuState cpuState, IMemory memory)
        {
            return cpuState.IsFlagSet(CpuState.Flags.Carry);
        }
    }
}