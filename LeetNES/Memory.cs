using System;

namespace LeetNES
{
    public class Memory : IMemory
    {
        private ICartridge cartridge;
        private readonly byte[] ram;
        private byte PPU_Status_Register = 0x80; //vblank
        private byte spriteAddress;
        private byte[] sprite;
        private IPpu ppu;

        public Memory(IPpu ppu)
        {
            this.ppu = ppu;
            ram = new byte[ushort.MaxValue];
            sprite = new byte[0x100];
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
                    return ram[addr & 0x1FFF];
                }

                
                if (addr < 0x4000)
                {
                    // Registers
                    if (addr == 0x2002 /*ppu status*/)
                    {
                        
                        return ppu.StatusRegRead(); 

                    }
                    if (addr == 0x2004)
                    {
                        return ppu.SpriteIORead();
                    }
                    if (addr == 0x2007)
                    {

                    }
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
                    switch (addr)
                    {
                        case 0x2000:
                            ppu.CtrlReg1Write((byte)addr);
                            break;
                        case 0x2001:
                            ppu.CtrlReg2Write((byte)addr);
                            break;
                        case 0x2003:
                            ppu.SpriteRegWrite((byte)addr);
                            break;
                        case 0x2004:
                            //ppu.SpriteramIOWrite(addr);
                            break;
                        case 0x2005:
                            //ppu.VRAMReg1Write(addr);
                            break;
                        case 0x2006:
                            ppu.VRAMReg2Write((byte)addr);
                            break;
                        case 0x2007:
                            ppu.VramIOWrite((byte)addr);
                            break;
                    }
                    
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