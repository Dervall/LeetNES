using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;

namespace LeetNES
{
    public class Cpu : ICpu
    {
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
                case 0x4C: // JMP absolute
                    
                    break;

                default:
                    throw new Exception("Unknown instruction encountered.");
            }
            return 0;
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