using LeetNES.ALU;
using LeetNES.ALU.Instructions;
using NUnit.Framework;
using Tests.Mock;

namespace Tests.ALU
{
    [TestFixture]
    public class TestCLD : InstructionTestBase<CLD>
    {
        [Test]
        public void TestCLDClearsFlagWhenSet()
        {
            mem.MapMemory(0, 0xD8);   
            cpuState.SetFlag(CpuState.Flags.DecimalMode, true);
            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(2, cycles);
            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.DecimalMode));
            Assert.AreEqual(1, cpuState.Pc);
        }

        [Test]
        public void TestCLDClearsFlagWhenNotSet()
        {
            mem.MapMemory(0, 0xD8);
            cpuState.SetFlag(CpuState.Flags.DecimalMode, false);
            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(2, cycles);
            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.DecimalMode));
            Assert.AreEqual(1, cpuState.Pc);
        }
    }
}
