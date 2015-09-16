using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace LeetNES
{    
    public class Cpu : ICpu
    {
        private enum Flags
        {
            C = 1,
            Z = 1 << 1,
            I = 1 << 2,
            D = 1 << 3,
            B = 1 << 4,
            V = 1 << 6,
            S = 1 << 7
        }

        private readonly IMemory mem;

        private ushort p;
        private byte a;
        private byte x;
        private byte y;
        private byte flags;
        private byte s;

        private int cycle;

        public Cpu(IMemory mem)
        {
            this.mem = mem;
        }

        public int Step()
        {
            byte opCode = mem[p];
            switch (opCode)
            {
             
                case 0xD8: // CLD
                    ClearFlag(Flags.D);
                    cycle += 2;
                    ++p;
                    break;
                case 0x78:  // SEI Set interrupt disable
                    SetFlag(Flags.I);
                    cycle += 2;
                    ++p;
                    break;
                default:
                    throw new Exception("Unknown instruction " + opCode.ToString("X") + " encountered.");
            }
            return 0;
        }

        private void SetFlag(Flags flag)
        {
            flags |= (byte)flag;
        }

        private void ClearFlag(Flags flag)
        {
            flags &= (byte)~flag;
        }

        public void Reset()
        {
            a = x = y = s = 0;
            p = (ushort) ((mem[0xFFFD] << 8) | mem[0xFFFC]);
            flags = 1 << 5;
            cycle = 0;
        }
    }
}