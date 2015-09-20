using LeetNES.ALU;
using LeetNES.ALU.Instructions;
using NUnit.Framework;
using Tests.Mock;

namespace Tests.ALU
{



    [TestFixture]
    public class TestADC : InstructionTestBase<ADC>
    {

        // ADC  Add Memory to Accumulator with Carry
        // 
        // A + M + C -> A, C                N Z C I D V
        //                                  + + + - - +
        // 
        // addressing    assembler    opc  bytes  cyles
        // --------------------------------------------
        // immidiate     ADC #oper     69    2     2        
        [Test]
        public void TestAddWithCarryImmediate()
        {
            cpuState.A = 0x0A;
            mem.MapMemory(0, 0x69, 0x10);

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(0x1A, cpuState.A);
            Assert.AreEqual(2, cycles);
            Assert.AreEqual(2, cpuState.Pc);

            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.Negative));
            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.Zero));
            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.Carry));
            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.Overflow));
        }

        [Test]
        public void TestAddWithCarryImmediateSetsCarry()
        {
            cpuState.A = 0x01;
            mem.MapMemory(0, 0x69, 0xFF);

            var cycles = instr.Execute(cpuState, mem);

            Assert.AreEqual(0x00, cpuState.A);
            Assert.AreEqual(2, cycles);
            Assert.AreEqual(2, cpuState.Pc);

            Assert.IsFalse(cpuState.IsFlagSet(CpuState.Flags.Negative));
            Assert.IsTrue(cpuState.IsFlagSet(CpuState.Flags.Zero));
            Assert.IsTrue(cpuState.IsFlagSet(CpuState.Flags.Carry));
            Assert.IsTrue(cpuState.IsFlagSet(CpuState.Flags.Overflow));
        }

        // zeropage      ADC oper      65    2     3
        //      [Test]
        //    public void Test
        


        // zeropage,X    ADC oper,X    75    2     4
        // absolute      ADC oper      6D    3     4
        // absolute,X    ADC oper,X    7D    3     4*
        // absolute,Y    ADC oper,Y    79    3     4*
        // (indirect,X)  ADC (oper,X)  61    2     6
        // (indirect),Y  ADC (oper),Y  71    2     5*
    }
}