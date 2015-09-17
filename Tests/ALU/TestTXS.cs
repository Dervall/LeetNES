using LeetNES.ALU;
using LeetNES.ALU.Instructions;
using Microsoft.Win32;
using NUnit.Framework;
using Tests.Mock;

namespace Tests.ALU
{
    [TestFixture]
    public class TestTXS : InstructionTestBase<TXS>
    {
        [Test]
        public void TestTransfersPositiveXToStackPointer()
        {
            mem.MapMemory(0, 0x9A);
            cpuState.X = 42;

            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(2, cycles);
            Assert.AreEqual(1, cpuState.Pc);
            Assert.AreEqual(42, cpuState.Sp);
            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.Zero));
            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.Negative));
        }

        [Test]
        public void TestTransfersZeroXToStackPointer()
        {
            mem.MapMemory(0, 0x9A);
            cpuState.Sp = 32;
            cpuState.X = 0;

            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(2, cycles);
            Assert.AreEqual(1, cpuState.Pc);
            Assert.AreEqual(0, cpuState.Sp);
            Assert.IsTrue(cpuState.IsFlagSet(CpuState.Flags.Zero));
            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.Negative));
        }

        [Test]
        public void TestTransfersNegativeXToStackPointer()
        {
            mem.MapMemory(0, 0x9A);
            cpuState.Sp = 32;
            cpuState.X = 0xF2;

            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(2, cycles);
            Assert.AreEqual(1, cpuState.Pc);
            Assert.AreEqual(0xF2, cpuState.Sp);
            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.Zero));
            Assert.IsTrue(cpuState.IsFlagSet(CpuState.Flags.Negative));
        }
    }
}
