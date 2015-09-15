using System;
using System.IO;
using System.Linq;

namespace LeetNES
{
    public class Cartridge : ICartridge
    {
        private readonly byte[] prgRom;
        private readonly byte[] chrMem;

        public Cartridge(string fileName)
        {
            using (var fileStream = File.OpenRead(fileName))
            {
                var header = new byte[16];
                fileStream.Read(header, 0, header.Length);
                if (!new byte[] {0x4E, 0x45, 0x53, 0x1A}.Zip(header.Take(4), (a, b) => a == b).All(f => f))
                {
                    throw new Exception("Not a valid iNES file");
                }

                int numRomBanks = header[4];
                int numVRomBanks = header[5];

                // TODO: Ignoring all the flags and all the settings.
                // TODO: Needs to handle mirroring and mapper types. We are assuming mapper 0
                // TODO: for the moment.

                prgRom = Read(fileStream, numRomBanks << 14);
                chrMem = Read(fileStream, numVRomBanks << 13); // TODO: Does not handle VRAM, only VROM               
                
                /*
                 0-3      String "NES^Z" used to recognize .NES files.
                4        Number of 16kB ROM banks.
                5        Number of 8kB VROM banks.
                6        bit 0     1 for vertical mirroring, 0 for horizontal mirroring.
                            bit 1     1 for battery-backed RAM at $6000-$7FFF.
                            bit 2     1 for a 512-byte trainer at $7000-$71FF.
                            bit 3     1 for a four-screen VRAM layout. 
                            bit 4-7   Four lower bits of ROM Mapper Type.
                7        bit 0     1 for VS-System cartridges.
                            bit 1-3   Reserved, must be zeroes!
                            bit 4-7   Four higher bits of ROM Mapper Type.
                8        Number of 8kB RAM banks. For compatibility with the previous
                            versions of the .NES format, assume 1x8kB RAM page when this
                            byte is zero.
                9        bit 0     1 for PAL cartridges, otherwise assume NTSC.
                            bit 1-7   Reserved, must be zeroes!
                10-15    Reserved, must be zeroes!
                16-...   ROM banks, in ascending order. If a trainer is present, its
                            512 bytes precede the ROM bank contents.
                ...-EOF  VROM banks, in ascending order.
                                    */

            }
        }

        private byte[] Read(Stream stream, int size)
        {
            var a = new byte[size];
            int offset = 0;
            while (size > 0)
            {
                var read = stream.Read(a, offset, size);
                if (read == -1)
                {
                    throw new Exception("End of file reached before buffer read was completed.");
                }
                size -= read;
                offset += read;
            }
            return a;
        }

        public byte ReadPrgRom(ushort addr)
        {
            return prgRom[addr];
        }

        public byte ReadChrMem(ushort addr)
        {
            return chrMem[addr];
        }
    }
}