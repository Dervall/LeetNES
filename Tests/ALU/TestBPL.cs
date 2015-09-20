using LeetNES.ALU;
using LeetNES.ALU.Instructions;
using NUnit.Framework;

namespace Tests.ALU
{
    [TestFixture]
    public class TestBPL : BranchInstructionTestBase<BPL>
    {
        protected override void SetBranchCondition(bool shouldBranch)
        {
            cpuState.SetFlag(CpuState.Flags.Negative, !shouldBranch);
        }
    }
}