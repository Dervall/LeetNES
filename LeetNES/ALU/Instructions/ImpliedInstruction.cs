namespace LeetNES.ALU.Instructions
{
    public abstract class ImpliedInstruction : IInstruction
    {
        public abstract byte OpCode { get; }
        public abstract string Mnemonic { get; }
        public int Size { get { return 1; } }
        public AddressingMode AddressingMode
        {
            get { return AddressingMode.Implied; }
        }

        public abstract int Execute(Cpu.State cpuState, IMemory memory);
    }
}