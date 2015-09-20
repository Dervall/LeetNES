using LeetNES.ALU.Instructions;
using NUnit.Framework;
using Tests.Mock;

namespace Tests.ALU
{
    /// <summary>
    /// LDX  Load Index X with Memory
    /// 
    /// M -> X                           N Z C I D V
    ///                                  + + - - - -
    ///  
    /// </summary>
    [TestFixture]
    public class TestLDX : InstructionTestBase<LDX>
    {
        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // immidiate     LDX #oper     A2    2     2
        [Test]
        public void TestLoadXImmediate()
        {
            mem.MapMemory(0, 0xA2, 0xDD);
            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(2, cycles);
            Assert.AreEqual(0xDD, cpuState.X);
            Assert.AreEqual(2, cpuState.Pc);
        }

        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // zeropage      LDX oper      A6    2     3
        [Test]
        public void TestLoadXZeroPage()
        {
            mem.MapMemory(0, 0xA6, 0xDD);
            mem.MapMemory(0xDD, 0xAB);
            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(3, cycles);
            Assert.AreEqual(0xAB, cpuState.X);
            Assert.AreEqual(2, cpuState.Pc);
        }
        
        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // zeropage,Y    LDX oper,Y    B6    2     4
        [Test]
        public void TestLoadXZeroPageYIndexed()
        {
            mem.MapMemory(0, 0xB6, 0xDD);
            mem.MapMemory(0xDF, 0xAB);
            cpuState.Y = 0x2; 

            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(4, cycles);
            Assert.AreEqual(0xAB, cpuState.X);
            Assert.AreEqual(2, cpuState.Pc);
        }

        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // absolute      LDX oper      AE    3     4
        [Test]
        public void TestLoadXAbsolute()
        {
            mem.MapMemory(0, 0xAE, 0x0D, 0xF0 );
            mem.MapMemory(0xF00D, 0xAB);

            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(4, cycles);
            Assert.AreEqual(0xAB, cpuState.X);
            Assert.AreEqual(3, cpuState.Pc);
        }

        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // absolute,Y    LDX oper,Y    BE    3     4*
        [Test]
        public void TestLoadXAbsoluteY()
        {
            mem.MapMemory(0, 0xBE, 0x00, 0xF0);
            mem.MapMemory(0xF00D, 0xAB);

            cpuState.Y = 0xD;

            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(4, cycles);
            Assert.AreEqual(0xAB, cpuState.X);
            Assert.AreEqual(3, cpuState.Pc);
        }

        [Test]
        public void TestLoadXAbsoluteYPageChange()
        {
            mem.MapMemory(0, 0xBE, 0xF1, 0xF0);
            mem.MapMemory(0xF100, 0xAB);

            cpuState.Y = 0x0F;

            var cycles = instr.Execute(cpuState, mem);
            Assert.AreEqual(5, cycles);
            Assert.AreEqual(0xAB, cpuState.X);
            Assert.AreEqual(3, cpuState.Pc);
        }
    }
}