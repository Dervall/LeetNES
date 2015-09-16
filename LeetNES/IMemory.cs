using System.Dynamic;

namespace LeetNES
{
    public interface IMemory
    {
        byte this[int addr] { get; set; }
        void SetCartridge(ICartridge cartridge);
    }
}