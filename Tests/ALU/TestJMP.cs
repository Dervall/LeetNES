using LeetNES.ALU.Instructions;
using NUnit.Framework;
using Tests.Mock;

namespace Tests.ALU
{
    /// <summary>
    /// JMP  Jump to New Location
    ///
    /// (PC+1) -> PCL                    N Z C I D V
    /// (PC+2) -> PCH                    - - - - - -
    ///
    /// addressing    assembler    opc  bytes  cyles
    /// --------------------------------------------
    /// absolute      JMP oper      4C    3     3
    /// indirect      JMP (oper)    6C    3     5
    /// </summary>
    [TestFixture]
    public class TestJMP : InstructionTestBase<JMP>
    {
        [Test]
        public void TestJumpAbsolute()
        {
            mem.MapMemory(0, 0x4C, 0x0D, 0xF0);

            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(3, cycles);
            Assert.AreEqual(0xF00D, cpuState.Pc);
        }

        [Test]
        public void TestJumpIndirect()
        {
            mem.MapMemory(0, 0x6C, 0x0D, 0xF0);
            mem.MapMemory(0xF00D, 0xEF, 0xBE);
            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(5, cycles);
            Assert.AreEqual(0xBEEF, cpuState.Pc);
        }
    }
}