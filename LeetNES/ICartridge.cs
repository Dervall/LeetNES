namespace LeetNES
{
    public interface ICartridge
    {
        byte ReadPrgRom(ushort addr);
        byte ReadChrMem(ushort addr);
        NametableMirroringMode NametableMirroring { get; set; }
    }
}