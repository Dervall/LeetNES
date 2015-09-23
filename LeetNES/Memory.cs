using System;

namespace LeetNES
{
    public class Memory : IMemory
    {
        private ICartridge cartridge;
        private readonly byte[] ram;
        private readonly IPpu ppu;
        private readonly Lazy<IIO> io;

        public Memory(IPpu ppu, Lazy<IIO> io)
        {
            this.ppu = ppu;
            this.io = io;
            ram = new byte[ushort.MaxValue];
        }

        public void SetCartridge(ICartridge cartridge)
        {
            this.cartridge = cartridge;
        }

        public byte ReadChrMem(ushort address)
        {
            return cartridge.ReadChrMem(address);
        }

        public byte this[int addr]
        {
            get
            {
                if (addr < 0x2000)
                {
                    return ram[addr & 0x1FFF];
                }

                
                if (addr < 0x4000)
                {
                    return ppu.StolenRead((ushort) (addr));
                }
                if (addr < 0x6000)
                {
                    if (addr == 0x4016 || addr == 0x4017)
                    {
                        return io.Value.ReadController(addr & 1);
                    }

                    // APU and expansion rom.
                    // TODO
                    return 0;
                }
                if (addr < 0x8000)
                {
                    // SRAM
                    // TODO
                    return 0;
                }
                
                return cartridge.ReadPrgRom((ushort) (addr & 0x3FFF));
            }

            set
            {
                if (addr < 0x2000)
                {
                    ram[addr & 0x1FFF] = value;
                }
                else if (addr < 0x4000)
                {
                    ppu.StolenWrite((ushort)(addr & 0xFFFF), value);

                }
                else if (addr < 0x4020)
                {
                    if (addr == 0x4016)
                    {
                        io.Value.SetStrobe((value & 1) == 1);
                    }
                }
                else if (addr < 0x8000)
                {
                    // SRAM
                    throw new NotImplementedException();
                }
                else
                {
                    // Writing to ROM? Impossiburu!
                    throw new Exception("No cookie for you");
                }
            }
        }
    }
}