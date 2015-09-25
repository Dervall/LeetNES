using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public abstract class BaseStoreInstruction : IInstruction
    {
        public abstract string Mnemonic { get; }
        public abstract IReadOnlyDictionary<byte, AddressingMode> Variants { get; }

        public int Execute(CpuState cpuState, IMemory memory)
        {
            var addressMode = Variants[memory[cpuState.Pc]];
            int cycles;

            Action<byte> write;

            switch (addressMode)
            {
                case AddressingMode.Accumulator:
                    write = b => cpuState.A = b;
                    cycles = 2;
                    break;
                case AddressingMode.Implied:
                    cycles = 2;
                    write = b => { };
                    break;
                case AddressingMode.Immediate:
                    {
                        var addr = cpuState.Pc + 1;
                        write = b => memory[addr] = b;
                        cycles = 2;
                        break;
                    }
                case AddressingMode.Absolute:
                    {
                        var addr = memory.ReadShort(cpuState.Pc + 1);
                        write = b => memory[addr] = b;
                        cycles = 4;
                        break;
                    }
                case AddressingMode.ZeroPage:
                    {
                        var addr = memory[cpuState.Pc + 1];
                        write = b => memory[addr] = b;
                        cycles = 3;
                        break;
                    }
                case AddressingMode.ZeroPageXIndexed:
                    {
                        var addr = (memory[cpuState.Pc + 1] + cpuState.X) & 0xFF;
                        write = b => memory[addr] = b;
                        cycles = 4;
                        break;
                    }
                case AddressingMode.ZeroPageYIndexed:
                    {
                        var addr = (memory[cpuState.Pc + 1] + cpuState.Y) & 0xFF;
                        write = b => memory[addr] = b;
                        cycles = 4;
                        break;
                    }
                case AddressingMode.AbsoluteX:
                    GetAbsoluteOffsetArg(cpuState, memory, cpuState.X, out cycles, out write);
                    break;
                case AddressingMode.AbsoluteY:
                    GetAbsoluteOffsetArg(cpuState, memory, cpuState.Y, out cycles, out write);
                    break;
                case AddressingMode.XIndexedIndirect:
                    {
                        var addr = memory.ReadZeroPageShort((byte) ((memory[cpuState.Pc + 1] + cpuState.X) & 0xFF));
                        write = b => memory[addr] = b;
                        cycles = 6;
                        break;
                    }
                case AddressingMode.IndirectYIndexed:
                    var addrPreOffset = memory.ReadZeroPageShort(memory[cpuState.Pc + 1]);
                    var addrPostOffset = addrPreOffset + cpuState.Y;
                    cycles = 5;
                    if ((addrPostOffset & 0xFF00) != (addrPreOffset & 0xFF00))
                    {
                        ++cycles;
                    }
                    write = b => memory[addrPostOffset] = b;
                    break;
                default:
                    throw new Exception("Unimplemented addressing mode");
            }

            InternalExecute(cpuState, memory, write, ref cycles);

            cpuState.Pc += addressMode.InstructionSize();

            return cycles;
        }

        private static void GetAbsoluteOffsetArg(CpuState cpuState, IMemory memory, byte offset, out int cycles, out Action<byte> write)
        {
            var address = memory.ReadShort(cpuState.Pc + 1);
            var offsetAddress = address + offset;
            cycles = 4;
            if ((offsetAddress & 0xFF00) != (address & 0xFF00))
            {
                ++cycles;
            }
            write = b => memory[offsetAddress] = b;
        }

        protected abstract void InternalExecute(CpuState cpuState, IMemory memory, Action<byte> write, ref int cycles);
    }

    /*STA  Store Accumulator in Memory

     A -> M                           N Z C I D V
                                      - - - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     zeropage      STA oper      85    2     3
     zeropage,X    STA oper,X    95    2     4
     absolute      STA oper      8D    3     4
     absolute,X    STA oper,X    9D    3     5
     absolute,Y    STA oper,Y    99    3     5
     (indirect,X)  STA (oper,X)  81    2     6
     (indirect),Y  STA (oper),Y  91    2     6
     */
    public class STA : BaseStoreInstruction
    {
        private static readonly IReadOnlyDictionary<byte, AddressingMode> addressingModes = new Dictionary<byte, AddressingMode>
        {
            {0x85, AddressingMode.ZeroPage},
            {0x95, AddressingMode.ZeroPageXIndexed},
            {0x8D, AddressingMode.Absolute},
            {0x9D, AddressingMode.AbsoluteX},
            {0x99, AddressingMode.AbsoluteY},
            {0x81, AddressingMode.XIndexedIndirect},
            {0x91, AddressingMode.IndirectYIndexed},
        };

        public override string Mnemonic
        {
            get { return "STA"; }
        }

        public override IReadOnlyDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return addressingModes;
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, Action<byte> write, ref int cycles)
        {
            write(cpuState.A);

            switch (Variants[memory[cpuState.Pc]])
            {
                case AddressingMode.AbsoluteX:
                case AddressingMode.AbsoluteY:
                    cycles = 5;
                    break;
                case AddressingMode.XIndexedIndirect:
                case AddressingMode.IndirectYIndexed:
                    cycles = 6;
                    break;
            }
        }
    }
}