namespace LeetNES.ALU.Instructions
{
    public abstract class ImmediateInstruction : IInstruction
    {
        public abstract byte OpCode { get; }
        public abstract string Mnemonic { get; }
        public int Size { get { return 2; } }
        public AddressingMode AddressingMode
        {
            get { return AddressingMode.Immediate; }
        }

        public int Execute(Cpu.State cpuState, IMemory memory)
        {
            return Execute(cpuState, memory, memory[(ushort) (cpuState.p + 1)]);
        }

        protected abstract int Execute(Cpu.State cpuState, IMemory memory, byte arg);
    }
}