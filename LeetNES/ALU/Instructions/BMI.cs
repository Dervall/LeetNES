namespace LeetNES.ALU.Instructions
{
    public class BMI : BaseBranchInstruction
    {
        public override string Mnemonic
        {
            get { return "BMI"; }
        }

        protected override byte OpCode
        {
            get { return 0x30; }
        }

        protected override bool ShouldBranch(CpuState cpuState, IMemory memory)
        {
            return cpuState.IsFlagSet(CpuState.Flags.Negative);
        }
    }
}