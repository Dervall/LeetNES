using System.Dynamic;

namespace LeetNES
{
    public interface IMemory
    {
        byte this[ushort addr] { get; set; }
        void SetCartridge(ICartridge cartridge);
    }
}