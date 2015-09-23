﻿namespace LeetNES
{
    public interface IEmulator
    {
        void Step();
        void Reset();
        void LoadCartridge(ICartridge cartridge);
    }
}