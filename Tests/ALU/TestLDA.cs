using LeetNES.ALU.Instructions;
using NUnit.Framework;
using Tests.Mock;

namespace Tests.ALU
{
    /// <summary>
    /// LDA  Load Accumulator with Memory
    ///
    /// M -> A                           N Z C I D V
    ///                                  + + - - - -
    // addressing    assembler    opc  bytes  cyles
    // --------------------------------------------
    /// </summary>
    [TestFixture]
    public class TestLDA : InstructionTestBase<LDA>
    {
        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // immidiate     LDA #oper     A9    2     2
        [Test]
        public void TestLoadAccumulatorImmediate()
        {
            mem.MapMemory(0, 0xA9, 0xCC);

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(2, cycles);
            Assert.AreEqual(2, cpuState.Pc);
            Assert.AreEqual(0xCC, cpuState.A);
        }


        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // zeropage      LDA oper      A5    2     3
        [Test]
        public void TestLoadAccumulatorZeroPage()
        {
            mem.MapMemory(0, 0xA5, 0xCC);
            mem.MapMemory(0xCC, 0xDD);   // Address to read from on the zero page

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(3, cycles);
            Assert.AreEqual(2, cpuState.Pc);
            Assert.AreEqual(0xDD, cpuState.A);
        }

        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // zeropage,X    LDA oper,X    B5    2     4
        [Test]
        public void TestLoadAccumulatorZeroPageX()
        {
            mem.MapMemory(0, 0xB5, 0xCC);
            mem.MapMemory(0xCE, 0xDD);   // Address to read from on the zero page CC + offset 2
            cpuState.X = 2;

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(4, cycles);
            Assert.AreEqual(2, cpuState.Pc);
            Assert.AreEqual(0xDD, cpuState.A);
        }

        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // absolute      LDA oper      AD    3     4
        [Test]
        public void TestLoadAccumulatorAbsolute()
        {
            mem.MapMemory(0, 0xAD, 0x0D, 0xF0);
            mem.MapMemory(0xF00D, 0xDD);   

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(4, cycles);
            Assert.AreEqual(3, cpuState.Pc);
            Assert.AreEqual(0xDD, cpuState.A);
        }

        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // absolute,X    LDA oper,X    BD    3     4*
        [Test]
        public void TestLoadAccumulatorAbsoluteX()
        {
            mem.MapMemory(0, 0xBD, 0x00, 0xF0);
            mem.MapMemory(0xF00D, 0xDD);
            cpuState.X = 0xD;

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(4, cycles);
            Assert.AreEqual(3, cpuState.Pc);
            Assert.AreEqual(0xDD, cpuState.A);
        }

        [Test]
        public void TestLoadAccumulatorAbsoluteXCrossPageBoundary()
        {
            mem.MapMemory(0, 0xBD, 0xFF, 0xF0);
            mem.MapMemory(0xF100, 0xDD);
            cpuState.X = 1;

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(5, cycles);
            Assert.AreEqual(3, cpuState.Pc);
            Assert.AreEqual(0xDD, cpuState.A);
        }

        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // absolute,Y    LDA oper,Y    B9    3     4*
        [Test]
        public void TestLoadAccumulatorAbsoluteY()
        {
            mem.MapMemory(0, 0xB9, 0x00, 0xF0);
            mem.MapMemory(0xF00D, 0xDD);
            cpuState.Y = 0xD;

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(4, cycles);
            Assert.AreEqual(3, cpuState.Pc);
            Assert.AreEqual(0xDD, cpuState.A);
        }

        [Test]
        public void TestLoadAccumulatorAbsoluteYCrossPageBoundary()
        {
            mem.MapMemory(0, 0xB9, 0xFF, 0xF0);
            mem.MapMemory(0xF100, 0xDD);
            cpuState.Y = 1;

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(5, cycles);
            Assert.AreEqual(3, cpuState.Pc);
            Assert.AreEqual(0xDD, cpuState.A);
        }


        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // (indirect,X)  LDA (oper,X)  A1    2     6
        [Test]
        public void TestLoadAccumulatorXIndexedIndirect()
        {
            mem.MapMemory(0, 0xA1, 0x30);
            mem.MapMemory(0x32, 0x0D, 0xF0);
            mem.MapMemory(0xF00D, 0xDD);
            cpuState.X = 2;

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(6, cycles);
            Assert.AreEqual(2, cpuState.Pc);
            Assert.AreEqual(0xDD, cpuState.A);
        }

        [Test]
        public void TestLoadAccumulatorXIndexedIndirectWithWraparound()
        {
            mem.MapMemory(0, 0xA1, 0x30);
            mem.MapMemory(0x2F, 0x0D, 0xF0);
            mem.MapMemory(0xF00D, 0xDD);
            cpuState.X = 0xFF;

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(6, cycles);
            Assert.AreEqual(2, cpuState.Pc);
            Assert.AreEqual(0xDD, cpuState.A);
        }

        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // (indirect),Y  LDA (oper),Y  B1    2     5*
        [Test]
        public void TestLoadAccumulatorIndexedIndirectY()
        {
            mem.MapMemory(0, 0xB1, 0x30);
            mem.MapMemory(0x30, 0x00, 0xF0);
            mem.MapMemory(0xF00D, 0xDD);
            cpuState.Y = 0x0D;

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(5, cycles);
            Assert.AreEqual(2, cpuState.Pc);
            Assert.AreEqual(0xDD, cpuState.A);
        }

        [Test]
        public void TestLoadAccumulatorIndexedIndirectYPageChange()
        {
            mem.MapMemory(0, 0xB1, 0x30);
            mem.MapMemory(0x30, 0xFF, 0xF0);
            mem.MapMemory(0xF100, 0xDD);
            cpuState.Y = 0x01;

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(6, cycles);
            Assert.AreEqual(2, cpuState.Pc);
            Assert.AreEqual(0xDD, cpuState.A);
        }
    }
}