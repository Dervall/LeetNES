using System;

namespace LeetNES
{
    public class Memory : IMemory
    {
        private ICartridge cartridge;
        private readonly byte[] ram;
        private IPpu ppu;

        public Memory(IPpu ppu)
        {
            this.ppu = ppu;
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
                    return ram[addr & 0x1FFF];
                }

                
                if (addr < 0x4000)
                {
                    return ppu.StolenRead((ushort) (addr ));
               ///*     // Registers
               //     if (addr == 0x2002 /*ppu status*/)
               //     {
                        
               //         return ppu.StatusRegRead(); 

               //     }
               //     if (addr == 0x2004)
               //     {
               //         return ppu.SpriteIORead();
               //     }
               //     if (addr == 0x2007)
               //     {
               //         return ppu.VRAMNametableRead();
               //     }
               //     // TODO
               //     return 0;*/
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
                    ram[addr & 0x1FFF] = value;
                }
                else if (addr < 0x4000)
                {
                    ppu.StolenWrite((ushort)(addr & 0xFFFF), value);
                    /*         switch (addr)
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
                            ppu.VRAMNametableWrite((byte)addr);
                            break;
                    }*/

                }
                else if (addr < 0x4020)
                {
                    // Ignore APU registers
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