namespace LeetNES.ALU.Instructions
{
    public interface IInstruction
    {        
        byte OpCode { get; }
        string Mnemonic { get; }
        int Size { get; }
        AddressingMode AddressingMode { get; }

        int Execute(Cpu.State cpuState, IMemory memory);
    }
}