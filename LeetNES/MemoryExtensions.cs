using System.Collections.Generic;

namespace LeetNES.ALU
{
    public static class MemoryExtensions
    {
        public static IEnumerable<byte> SequenceFrom(this IMemory mem, ushort addr)
        {
            for (;;)
            {
                yield return mem[addr++];
            }
        }
    }
}