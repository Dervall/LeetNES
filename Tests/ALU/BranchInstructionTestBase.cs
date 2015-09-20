using System.Linq;
using LeetNES.ALU.Instructions;
using NUnit.Framework;
using Tests.Mock;

namespace Tests.ALU
{
    public abstract class BranchInstructionTestBase<T> : InstructionTestBase<T> where T : BaseBranchInstruction, new()
    {
        [Test]
        public void TestBranchDoesNotOccur()
        {
            mem.MapMemory(0, instr.Variants.First().Key, 0x0D);
            SetBranchCondition(false);;
            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(2, cycles);
            Assert.AreEqual(2, cpuState.Pc);
        }

        [Test]
        public void TestBranchDoesOccurSamePage()
        {
            mem.MapMemory(0, instr.Variants.First().Key, 0x0D);
            SetBranchCondition(true); ;
            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(3, cycles);
            Assert.AreEqual(0xF, cpuState.Pc);
        }

        [Test]
        public void TestBranchDoesOccurNegativeOffset()
        {
            mem.MapMemory(0, instr.Variants.First().Key, 0xFF);
            SetBranchCondition(true); ;
            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(1, cpuState.Pc);
            Assert.AreEqual(3, cycles);
        }

        [Test]
        public void TestBranchDoesOccurPageChange()
        {
            mem.MapMemory(0x7F, instr.Variants.First().Key, 0x7F);
            cpuState.Pc = 0x7F;
            SetBranchCondition(true);
            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(0x100, cpuState.Pc);
            Assert.AreEqual(4, cycles);
        }

        protected abstract void SetBranchCondition(bool shouldBranch);
    }
}