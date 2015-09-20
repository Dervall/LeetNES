using System;

namespace LeetNES
{
    public interface IPpu
    {
        void Step(int ppuCycles);
        void CtrlReg1Write(int addr);
        void CtrlReg2Write(int addr);
        void SpriteramRegWrite(int addr);
        void SpriteramIOWrite(int addr);
        void VRAMReg1Write(int addr);
        void VRAMReg2Write(int addr);
        void VramIOWrite(int addr);
    }

    public class Ppu : IPpu
    {
        private IMemory memory;
        //public void Control_Register_1_Write(byte data)
        //{
        //    //go bit by bit, and flag our values
        //    if ((data & 0x80) == 0x80)
        //        executeNMIonVBlank = true;
        //    else
        //        executeNMIonVBlank = false;

        //    if ((data & 0x20) == 0x20)
        //        spriteSize = 16;
        //    else
        //        spriteSize = 8;

        //    if ((data & 0x10) == 0x10)
        //        backgroundAddress = 0x1000;
        //    else
        //        backgroundAddress = 0x0000;

        //    if ((data & 0x8) == 0x8)
        //        spriteAddress = 0x1000;
        //    else
        //        spriteAddress = 0x0000;

        //    if ((data & 0x4) == 0x4)
        //        ppuAddressIncrement = 32;
        //    else
        //        ppuAddressIncrement = 1;
        //    if ((backgroundVisible == true) || (ppuMaster == 0xff) || (ppuMaster == 1))
        //    {
        //        switch (data & 0x3)
        //        {
        //            case (0x0): nameTableAddress = 0x2000; break;
        //            case (0x1): nameTableAddress = 0x2400; break;
        //            case (0x2): nameTableAddress = 0x2800; break;
        //            case (0x3): nameTableAddress = 0x2C00; break;
        //        }
        //    }
        //    /*if (myEngine.fix_bgchange == true)
        //    {
        //        if (currentScanline == 241)
        //            nameTableAddress = 0x2000;
        //    }*/

        //    if (ppuMaster == 0xff)
        //    {
        //        if ((data & 0x40) == 0x40)
        //            ppuMaster = 0;
        //        else
        //            ppuMaster = 1;
        //    }
        //}
        //public void Control_Register_2_Write(byte data)
        //{
        //    if ((data & 0x1) == 0x1)
        //        monochromeDisplay = true;
        //    else
        //        monochromeDisplay = false;
        //    if ((data & 0x2) == 0x2)
        //        noBackgroundClipping = true;
        //    else
        //        noBackgroundClipping = false;

        //    if ((data & 0x4) == 0x4)
        //        noSpriteClipping = true;
        //    else
        //        noSpriteClipping = false;

        //    if ((data & 0x8) == 0x8)
        //        backgroundVisible = true;
        //    else
        //        backgroundVisible = false;

        //    if ((data & 0x10) == 0x10)
        //        spritesVisible = true;
        //    else
        //        spritesVisible = false;

        //    ppuColor = (data >> 5);

        //}
        public void Step(int ppuCycles)
        {

        }
#region commands
        public void CtrlReg1Write(int addr)
        {
            throw new NotImplementedException();
        }

        public void CtrlReg2Write(int addr)
        {
            throw new NotImplementedException();
        }

        public void SpriteramIOWrite(int addr)
        {
            throw new NotImplementedException();
        }

        public void SpriteramRegWrite(int addr)
        {
            throw new NotImplementedException();
        }



        public void VramIOWrite(int addr)
        {
            throw new NotImplementedException();
        }

        public void VRAMReg1Write(int addr)
        {
            throw new NotImplementedException();
        }

        public void VRAMReg2Write(int addr)
        {
            throw new NotImplementedException();
        }
#endregion
    }
}