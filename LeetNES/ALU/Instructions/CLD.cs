namespace LeetNES.ALU.Instructions
{
    public class CLD : ImpliedInstruction
    {
        public override byte OpCode { get { return 0xD8; }}
        public override string Mnemonic { get { return "CLD"; } }
        
        public override int Execute(Cpu.State cpuState, IMemory memory)
        {
            cpuState.SetFlag(Cpu.Flags.DecimalMode, false);
            cpuState.p++;
            return 2;
        }
    }
}