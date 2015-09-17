using System.Net.Mail;
using LeetNES;
using LeetNES.ALU;
using LeetNES.ALU.Instructions;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.ALU
{
    public class InstructionTestBase<T> where T : IInstruction, new()
    {
        protected T instr;
        protected CpuState cpuState;
        protected IMemory mem;

        [SetUp]
        public void Setup()
        {
            instr = new T();
            cpuState = new CpuState();
            mem = MockRepository.GenerateStrictMock<IMemory>();
        }
    }
}