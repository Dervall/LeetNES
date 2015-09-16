namespace LeetNES.ALU.Instructions
{
    public class LDXImmediate : ImmediateInstruction
    {
        public override byte OpCode
        {
            get { return 0xA2; }
        }

        public override string Mnemonic
        {
            get { return "LDX"; }
        }

        protected override int Execute(Cpu.State cpuState, IMemory memory, byte arg)
        {
            cpuState.x = arg;
            cpuState.p += 2;
            cpuState.SetFlag(Cpu.Flags.Negative, 0x7000 & arg);
            cpuState.SetFlag(Cpu.Flags.Zero, arg == 0);
            return 2;
        }
    }
}