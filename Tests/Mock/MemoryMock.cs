using LeetNES;
using Rhino.Mocks;

namespace Tests.Mock
{
    public static class MemoryMock
    {
        public static void MapMemory(this IMemory mem, int address, params byte[] map)
        {
            for (int i = 0; i < map.Length; ++i)
            {
                var addr = i + address;

                mem.Expect(x => x[addr]).Return(map[i]);
            }
        }
    }
}