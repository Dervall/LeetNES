using LeetNES.ALU;
using LeetNES.ALU.Instructions;
using NUnit.Framework;
using Tests.Mock;

namespace Tests.ALU
{
    [TestFixture]
    public class TestSEI : InstructionTestBase<SEI>
    {
        [Test]
        public void TestSetsInterruptDisableFlag()
        {
            mem.MapMemory(0, 0x78);
            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.InterruptDisable));

            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(2, cycles);
            Assert.IsTrue(cpuState.IsFlagSet(CpuState.Flags.InterruptDisable));
            Assert.AreEqual(1, cpuState.Pc);
        }
    }
}