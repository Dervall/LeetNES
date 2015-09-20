using System;
using System.Diagnostics;
using System.Net;
using log4net.Appender;
using LeetNES.ALU;

namespace LeetNES
{
    public interface IPpu
    {
        void Step(int ppuCycles);
        void CtrlReg1Write(byte data);
        void CtrlReg2Write(byte data);
        void SpriteRegWrite(byte data);
        void SpriteIOWrite(byte data);
        byte VRAMNametableRead();
        void VRAMNametableWrite(byte data);
        void VRAMReg1Write(byte data);
        void VRAMReg2Write(byte data);
        byte StatusRegRead();
        byte SpriteIORead();

        byte StolenRead(ushort addr);
        void StolenWrite(ushort addr, byte val);
    }

    public enum MirrorMode
    {
        Vertical,
        Horizontal,
        SingleScreen
    }
    public class Ppu : IPpu
    {
        private MirrorMode mirrorMode;
        private readonly Lazy<ICpu> cpu;
        private readonly Lazy<IMemory> _memory;
        private readonly Lazy<StolenPpu> _ppu;
        private bool nmiOnVblank;
        private bool _16bitSpriteSize;
        private int bgAddr;
        private uint spriteAddr;
        private int ppuAddrIncr;
        private bool bgVisible;
        private int nameTableAddr;
        private byte ppuMaster; // 0 = slave, 1 = master, 0xff = unset (master) 
        private IMemory memory;
        private bool monochromeDisplay;
        private bool noBgClip;
        private bool noSpriteClip;
        private bool spritesVisible;
        private int ppuColor;
        private byte[] spriteRam;
        private int vramAddr;
        private byte[] nameTables;
        private int prev_vramAddr;
        private int vramHiLoToggle;
        private byte scrollH;
        private bool sprite0Hit;
        private int spritesCrossed;

        private int currentScanline = 0;
        private int x;
        private bool oddFrame = false;
        private bool inVblank = false;
        private byte scrollV;
        private bool fix_scrolloffset2;
        private bool fix_scrolloffset1;
        private bool fix_scrolloffset3;
        private byte vramReadBuffer;
      

        public Ppu(Lazy<ICpu> cpu, Lazy<IMemory> memory, Lazy<StolenPpu> ppu)
        {
            this.cpu = cpu;
            _memory = memory;
            _ppu = ppu;
            nameTables = new byte[1024];
            spriteRam = new byte[256];
            //Todo: Allow setting of mirrormode
            this.mirrorMode = MirrorMode.Vertical;
            
        }

        public void Step(int ppuCycles)
        {
            for(int i = 0; i < ppuCycles; ++i)
                _ppu.Value.step();
       /*     if (currentScanline == -1 && x == 0)
            {
                inVblank = false;
                sprite0Hit = false;
            }

            if (x < 256 && currentScanline >= 0 && currentScanline <= 240)
            {
                _ppu.Value.drawBackground(x, currentScanline);
            }
            // Render goes here

            ++x;
            // 241 starts the VBlank region.
            if (currentScanline == 241 && x == 1)
            {
                // At the very start of the VBlank region, let the CPU know.
                inVblank = true;

                // Potentially trigger NMI at the start of VBlank
                if (nmiOnVblank)
                {
                    cpu.Value.Nmi();
                }
            }


            // Variable line width for the pre-scanline depending on even/odd frame
            int columnsThisFrame = bgVisible && oddFrame && currentScanline == -1 ? 340 : 341;
            if (x == columnsThisFrame)
            {
                currentScanline++;
                x = 0;
            }

            if (currentScanline == 261)
            {
                currentScanline = -1;
                oddFrame = !oddFrame;
            }*/
        }

        #region registers

        #region read

        public byte SpriteIORead()
        {
            return spriteRam[spriteAddr];
        }

        public byte StolenRead(ushort addr)
        {
            return _ppu.Value.read(addr);
        }

        public void StolenWrite(ushort addr, byte val)
        {
            _ppu.Value.write(addr, val);
        }

        public byte StatusRegRead()
        {
            byte returnedValue = 0;

            // VBlank
            if (inVblank)
                returnedValue = (byte)(returnedValue | 0x80);

            if (sprite0Hit)
            {
                returnedValue = (byte)(returnedValue | 0x40);
            }
            // Sprites on current scanline
            if (spritesCrossed > 8)
                returnedValue = (byte)(returnedValue | 0x20);

            vramHiLoToggle = 1;

            return returnedValue;
        }

        #endregion
        public void CtrlReg1Write(byte data)
        {

            //Should we do a non-maskable interupt on vblank?
            nmiOnVblank = (data & 0x80) == 0x80;
            //Else infer 8bit;
            _16bitSpriteSize = (data & 0x20) == 0x20;
            bgAddr = (data & 0x10) == 0x10 ? 0x1000 : 0x0000;
            spriteAddr = (data & 0x8) == 0x8 ? 0x1000 : (uint)0x000;
            ppuAddrIncr = (data & 0x4) == 0x4 ? 32 : 1;
            if (bgVisible || ppuMaster != 0 /*unset or master*/)
            {
                switch (data & 0x3)
                {
                    case (0x0): nameTableAddr = 0x2000; break;
                    case (0x1): nameTableAddr = 0x2400; break;
                    case (0x2): nameTableAddr = 0x2800; break;
                    case (0x3): nameTableAddr = 0x2C00; break;
                }
            }
            //If ppu is unset, if data is 0x4.. set ppu to slave else master
            ppuMaster =
                ppuMaster == 0xff ?
                    ((data & 0x40) == 0x40 ?
                            (byte)0 :
                            (byte)1)
                    : ppuMaster;

        }
        public void CtrlReg2Write(byte data)
        {
            monochromeDisplay = (data & 0x1) == 0x1;
            noBgClip = (data & 0x2) == 0x2;
            noSpriteClip = (data & 0x4) == 0x4;
            bgVisible = (data & 0x8) == 0x8;
            spritesVisible = (data & 0x10) == 0x10;
            ppuColor = (data >> 5);
        }

        public void SpriteIOWrite(byte data)
        {
            spriteRam[spriteAddr] = data;
            spriteAddr++;
        }

        public byte VRAMNametableRead()
        {
            byte returnedValue = 0;

            if (vramAddr < 0x3f00)
            {
                returnedValue = vramReadBuffer;
                if (vramAddr >= 0x2000)
                {
                    vramReadBuffer = nameTables[vramAddr - 0x2000];
                }
                else
                {
                   // vramReadBuffer = /*Read from chrrom(vramAddr));*/
                }
            }
            else if (vramAddr >= 0x4000)
            {
                //Bör krasha, fel mirroring

            }
            else
            {
                returnedValue = nameTables[vramAddr - 0x2000];
            }
            vramAddr = vramAddr + ppuAddrIncr;
            return returnedValue;
        }


        public void SpriteRegWrite(byte addr)
        {
            spriteAddr = addr;
        }

        public void VRAMNametableWrite(byte data)
        {
            //Not implemented yet
            if (vramAddr < 0x200)
            {
                Debug.WriteLine("Should write to chrRom");
                /*Mapper Write ChrRom , vramAddr, data*/
            }
            else if (vramAddr >= 0x2000 && vramAddr < 0x3f00)
            {
                //If mirroring

                //Horizontal Mirroring
                if (mirrorMode == MirrorMode.Horizontal)
                {
                    switch (vramAddr & 0x2c00)
                    {
                        case (0x2000):
                            nameTables[vramAddr - 0x2000] = data;
                            break;
                        case (0x2400):
                            nameTables[(vramAddr - 0x400) - 0x2000] = data;
                            break;
                        case (0x2800):
                            nameTables[vramAddr - 0x400 - 0x2000] = data;
                            break;
                        case (0x2C00):
                            nameTables[(vramAddr - 0x800) - 0x2000] = data;
                            break;
                    }
                }
                //vertical
                else if (mirrorMode == MirrorMode.Vertical)
                {

                    switch (vramAddr & 0x2c00)
                    {
                        case (0x2000):
                            nameTables[vramAddr - 0x2000] = data;
                            break;
                        case (0x2400):
                            nameTables[vramAddr - 0x2000] = data;
                            break;
                        case (0x2800):
                            nameTables[vramAddr - 0x800 - 0x2000] = data;
                            break;
                        case (0x2C00):
                            nameTables[(vramAddr - 0x800) - 0x2000] = data;
                            break;
                    }
                }

                //onescreen 0x2000 base
                //switch (vramAddr & 0x2C00)
                //{
                //    case (0x2000):
                //        nameTables[vramAddr - 0x2000] = data;
                //        break;
                //    case (0x2400):
                //        nameTables[vramAddr - 0x400 - 0x2000] = data;
                //        break;
                //    case (0x2800):
                //        nameTables[vramAddr - 0x800 - 0x2000] = data;
                //        break;
                //    case (0x2C00):
                //        nameTables[vramAddr - 0xC00 - 0x2000] = data;
                //        break;
                //}

                //Onescreen 0x2400 base
                //switch (vramAddr & 0x2C00)
                //{
                //    case (0x2000):
                //        nameTables[vramAddr + 0x400 - 0x2000] = data;
                //        break;
                //    case (0x2400):
                //        nameTables[vramAddr - 0x2000] = data;
                //        break;
                //    case (0x2800):
                //        nameTables[vramAddr - 0x400 - 0x2000] = data;
                //        break;
                //    case (0x2C00):
                //        nameTables[vramAddr - 0x800 - 0x2000] = data;
                //        break;
                //}

                //if vramAddr >= 0x3f00 && vramAddr < 0x3f20
                else if (vramAddr >= 0x3f00 && vramAddr < 0x3f20)
                {

                    nameTables[vramAddr - 0x2000] = data;
                    if ((vramAddr & 0x7) == 0)
                    {
                        nameTables[(vramAddr - 0x2000) ^ 0x10] = data;
                    }
                }
            }
            vramAddr = vramAddr + ppuAddrIncr;
        }

        public void VRAMReg1Write(byte data)
        {


            if (vramHiLoToggle == 1)
            {
                scrollV = data;
                vramHiLoToggle = 0;
            }
            else
            {
                scrollH = data;
                if (scrollH > 239)
                {
                    scrollH = 0;
                }
                if (fix_scrolloffset2)
                {
                    if (currentScanline < 240)
                    {
                        scrollH = (byte) (scrollH - currentScanline + 8);
                    }
                }
                if (fix_scrolloffset1)
                {
                    if (currentScanline < 240)
                    {
                        scrollH = (byte) (scrollH - currentScanline);
                    }
                }
                if (fix_scrolloffset3)
                {
                    if (currentScanline < 240)
                        scrollH = 238;
                }
                vramHiLoToggle = 1;
            }
        }

        public void VRAMReg2Write(byte data)
        {
            if (vramHiLoToggle == 1)
            {
                prev_vramAddr = vramAddr;
                vramAddr = data << 8;
                vramHiLoToggle = 0;
            }
            else
            {
                vramAddr = vramAddr + data;
                if ((prev_vramAddr == 0) && (currentScanline < 240))
                {
                    if ((vramAddr >= 0x2000) && (vramAddr <= 0x2400))
                        scrollH = (byte)(((vramAddr - 0x2000) / 0x20) * 8 - currentScanline);
                }
                vramHiLoToggle = 1;
            }
        }

      
        #endregion
    }
}