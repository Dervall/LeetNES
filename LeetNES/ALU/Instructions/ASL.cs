using System;
using System.Collections.Generic;

namespace LeetNES.ALU.Instructions
{
    public class ASL : BaseInstruction
    {
        public override string Mnemonic
        {
            get { return "ASL"; }
        }

        public override IDictionary<byte, AddressingMode> Variants
        {
            get
            {
                return new Dictionary<byte, AddressingMode>
                {
                    { 0x0A, AddressingMode.Accumulator },
                    { 0x06, AddressingMode.ZeroPage },
                    { 0x16, AddressingMode.ZeroPageXIndexed },
                    { 0x0E, AddressingMode.Absolute },
                    { 0x1E, AddressingMode.AbsoluteX }
                };
            }
        }

        protected override void InternalExecute(CpuState cpuState, IMemory memory, byte arg, Action<byte> write, ref int cycles)
        {
            ushort result = arg;
            result <<= 1;
            var byteResult = (byte) (result & 0xFF);
            write(byteResult);
            cpuState.SetNegativeFlag(byteResult);
            cpuState.SetZeroFlag(byteResult);
            cpuState.SetFlag(CpuState.Flags.Carry, result & 0xFF00);

            switch (Variants[memory[cpuState.Pc]])
            {
                case AddressingMode.Accumulator:
                    cycles = 2;
                    break;
                case AddressingMode.ZeroPage:
                    cycles = 5;
                    break;
                case AddressingMode.ZeroPageXIndexed:
                    cycles = 6;
                    break;
                case AddressingMode.Absolute:
                    cycles = 6;
                    break;
                case AddressingMode.AbsoluteX:
                    cycles = 7;
                    break;
            }
        }
    }
}