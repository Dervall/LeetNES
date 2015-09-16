using System.Collections.Generic;

namespace LeetNES
{
    public static class MemoryExtensions
    {
        /// <summary>
        /// Read a little endian short from a given address.
        /// </summary>
        /// <param name="mem">Memory to read</param>
        /// <param name="addr">Address to read from</param>
        /// <returns></returns>
        public static ushort ReadShort(this IMemory mem, int addr)
        {
            return (ushort) (mem[addr] | (mem[addr + 1] << 8));
        }

        public static IEnumerable<byte> SequenceFrom(this IMemory mem, ushort addr)
        {
            for (;;)
            {
                yield return mem[addr++];
            }
        }
    }
}