using LeetNES.ALU;

namespace LeetNES
{
    public class Emulator : IEmulator
    {
        private readonly ICpu cpu;
        private readonly IPpu ppu;

        public Emulator(ICpu cpu, IPpu ppu)
        {
            this.cpu = cpu;
            this.ppu = ppu;
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
    }
}