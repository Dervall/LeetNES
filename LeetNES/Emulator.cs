using LeetNES.ALU;

namespace LeetNES
{
    public class Emulator : IEmulator
    {
        private readonly ICpu cpu;
        private readonly IPpu ppu;
        private readonly IMemory memory;

        public Emulator(ICpu cpu, IPpu ppu, IMemory memory)
        {
            this.cpu = cpu;
            this.ppu = ppu;
            this.memory = memory;
        }

        public void Step()
        {
            int cycles = cpu.Step();
            ppu.Step(cycles * 3);
        }

        public void Reset()
        {
            cpu.Reset();
        }

        public void LoadCartridge(ICartridge cartridge)
        {
            memory.SetCartridge(cartridge);
        }
    }
}