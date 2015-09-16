using System;

namespace LeetNES
{
    public class Memory : IMemory
    {
        private ICartridge cartridge;
        private readonly byte[] ram;

        public Memory()
        {
            ram = new byte[ushort.MaxValue];
        }

        public void SetCartridge(ICartridge cartridge)
        {
            this.cartridge = cartridge;
        }

        public byte this[int addr]
        {
            get
            {
                if (addr < 0x2000)
                {
                    return ram[addr & 0x800];
                }
                if (addr < 0x4000)
                {
                    // Registers
                    // TODO
                    return 0;
                }
                if (addr < 0x6000)
                {
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
                    ram[addr & 0x800] = value;
                }
                else if (addr < 0x4000)
                {
                    // Registers
                    throw new NotImplementedException();
                }
                else if (addr < 0x6000)
                {
                    // APU and expansion rom.
                    throw new NotImplementedException();
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