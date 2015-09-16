using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetNES
{
    public enum AddressingMode
    {
        Accumulator,        // OPC A	 	    operand is AC
        Absolute,           // OPC $HHLL	    operand is address $HHLL
        AbsoluteX,          // OPC $HHLL,X	 	operand is address incremented by X with carry
        AbsoluteY,          // OPC $HHLL,Y	 	operand is address incremented by Y with carry
        Immediate,          // OPC #$BB	 	    operand is byte (BB)
        Implied,            // OPC	 	        operand implied
        Indirect,           // OPC ($HHLL)	 	operand is effective address; effective address is value of address
        XIndexedIndirect,   // OPC ($BB,X)	 	operand is effective zeropage address; effective address is byte (BB) incremented by X without carry
        IndirectYIndexed,   // OPC ($LL),Y	 	operand is effective address incremented by Y with carry; effective address is word at zeropage address
        Relative,           // OPC $BB	 	    branch target is PC + offset (BB), bit 7 signifies negative offset
        ZeroPage,           // OPC $LL	 	    operand is of address; address hibyte = zero ($00xx)
        ZeroPageXIndexed,   // OPC $LL,X	 	operand is address incremented by X; address hibyte = zero ($00xx); no page transition
        ZeroPageYIndexed,   // OPC $LL,Y	 	operand is address incremented by Y; address hibyte = zero ($00xx); no page transition
    }

    public static class InstructionExtensions
    {
        public static string Disassemble(this IInstruction instruction, byte[] args)
        {
            switch (instruction.AddressingMode)
            {
                case AddressingMode.Accumulator:
                    return string.Format("{0} A", instruction.Mnemonic);
                case AddressingMode.Absolute:
                    return string.Format("{0} ${1:X}{2:X}", instruction.Mnemonic, args[1], args[0]);
                case AddressingMode.AbsoluteX:
                    return string.Format("{0} ${1:X}{2:X},X", instruction.Mnemonic, args[1], args[0]);
                case AddressingMode.AbsoluteY:
                    return string.Format("{0} ${1:X}{2:X},Y", instruction.Mnemonic, args[1], args[0]);
                case AddressingMode.Immediate:
                    return string.Format("{0} #${1:X}", instruction.Mnemonic, args[0]);
                case AddressingMode.Implied:
                    return instruction.Mnemonic;
                case AddressingMode.Indirect:
                    return string.Format("{0} (${1:X}{2:X})", instruction.Mnemonic, args[1], args[0]);
                case AddressingMode.XIndexedIndirect:
                    return string.Format("{0} (${1:X},X)", instruction.Mnemonic, args[0]);
                case AddressingMode.IndirectYIndexed:
                    return string.Format("{0} (${1:X}),Y", instruction.Mnemonic, args[0]);
                case AddressingMode.Relative:
                    return string.Format("{0} ${1:X}", instruction.Mnemonic, args[0]);
                case AddressingMode.ZeroPage:
                    return string.Format("{0} ${1:X}", instruction.Mnemonic, args[0]);
                case AddressingMode.ZeroPageXIndexed:
                    return string.Format("{0} ${1:X},X", instruction.Mnemonic, args[0]);
                case AddressingMode.ZeroPageYIndexed:
                    return string.Format("{0} ${1:X},Y", instruction.Mnemonic, args[0]);
                default:
                    throw new Exception("Disassembly for addressing mode is unimplemented");
            }
        }
    }

    public interface IInstruction
    {        
        byte OpCode { get; }
        string Mnemonic { get; }
        int Size { get; }
        AddressingMode AddressingMode { get; }

        int Execute(Cpu.State cpuState, IMemory memory);
    }

    public abstract class ImmediateInstruction : IInstruction
    {
        public abstract byte OpCode { get; }
        public abstract string Mnemonic { get; }
        public int Size { get { return 2; } }
        public AddressingMode AddressingMode
        {
            get { return AddressingMode.Immediate; }
        }

        public int Execute(Cpu.State cpuState, IMemory memory)
        {
            return Execute(cpuState, memory, memory[(ushort) (cpuState.p + 1)]);
        }

        protected abstract int Execute(Cpu.State cpuState, IMemory memory, byte arg);
    }

    public class LDXImmediate : ImmediateInstruction
    {
        public override byte OpCode
        {
            get { return 0xA2; }
        }

        public override string Mnemonic
        {
            get { return "LDX"; }
        }

        protected override int Execute(Cpu.State cpuState, IMemory memory, byte arg)
        {
            cpuState.x = arg;
            cpuState.p += 2;
            cpuState.SetFlag(Cpu.Flags.N, 0x7000 & arg);
            cpuState.SetFlag(Cpu.Flags.Z, arg == 0);
            return 2;
        }
    }

    public abstract class ImpliedInstruction : IInstruction
    {
        public abstract byte OpCode { get; }
        public abstract string Mnemonic { get; }
        public int Size { get { return 1; } }
        public AddressingMode AddressingMode
        {
            get { return AddressingMode.Implied; }
        }

        public abstract int Execute(Cpu.State cpuState, IMemory memory);
    }

    public class CLD : ImpliedInstruction
    {
        public override byte OpCode { get { return 0xD8; }}
        public override string Mnemonic { get { return "CLD"; } }
        
        public override int Execute(Cpu.State cpuState, IMemory memory)
        {
            cpuState.SetFlag(Cpu.Flags.D, false);
            cpuState.p++;
            return 2;
        }
    }

    public class TXS : ImpliedInstruction
    {
        public override byte OpCode
        {
            get { return 0x9A; }
        }

        public override string Mnemonic
        {
            get { return "TXS"; }
        }

        public override int Execute(Cpu.State cpuState, IMemory memory)
        {
            cpuState.s = cpuState.x;
            cpuState.SetFlag(Cpu.Flags.N, 0x7000 & cpuState.s);
            cpuState.SetFlag(Cpu.Flags.Z, cpuState.s == 0);
            cpuState.p += 1;
            return 2;
        }
    }

    public class SEI : ImpliedInstruction
    {
        public override byte OpCode
        {
            get { return 0x78; }
        }

        public override string Mnemonic
        {
            get { return "SEI"; }
        }

        public override int Execute(Cpu.State cpuState, IMemory memory)
        {
            cpuState.SetFlag(Cpu.Flags.I, true);
            cpuState.p++;
            return 2;
        }
    }

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

    public class Cpu : ICpu
    {
        public enum Flags
        {
            C = 1,
            Z = 1 << 1,
            I = 1 << 2,
            D = 1 << 3,
            B = 1 << 4,
            V = 1 << 6,
            N = 1 << 7
        }

        private readonly IMemory mem;

        public class State
        {
            public ushort p;
            public byte a;
            public byte x;
            public byte y;
            public byte flags;
            public byte s;

            public void SetFlag(Flags flag, int i)
            {
                SetFlag(flag, i != 0);                
            }

            public void SetFlag(Flags flag, bool b)
            {
                if (b)
                    flags |= (byte)flag;
                else
                    flags &= (byte)~flag;
            }
        }

        private int cycle;
        private readonly State state;
        private readonly Dictionary<byte, IInstruction> instructions;

        public Cpu(IMemory mem, IEnumerable<IInstruction> instructions)
        {
            this.mem = mem;
            state = new State();
            this.instructions = instructions.ToDictionary(f => f.OpCode, f => f);
        }

        public int Step()
        {
            byte opCode = mem[state.p];
            IInstruction instruction;
            if (!instructions.TryGetValue(opCode, out instruction))
            {
                throw new Exception("Unknown instruction " + opCode.ToString("X") + " encountered.");
            }

            Console.WriteLine("{0:X2}  {1,-10}{2,-32}A:{3:X2} X:{4:X2} Y:{5:X2} P:{6:X2} SP:{7:X2}", // TODO: Cycle and scanline goes at the end
                state.p, 
                string.Join(" ", mem.SequenceFrom(state.p).Take(instruction.Size).Select(x => x.ToString("X2"))), 
                instruction.Disassemble(mem.SequenceFrom((ushort)(state.p + 1)).Take(instruction.Size - 1).ToArray()),
                state.a,
                state.x,
                state.y,
                state.flags,
                state.s);

            var instructionTime = instruction.Execute(state, mem);
            cycle += instructionTime;
            return instructionTime;
        }

        public void Reset()
        {
            state.a = state.x = state.y = state.s = 0;
            state.p = (ushort) ((mem[0xFFFD] << 8) | mem[0xFFFC]);
            state.flags = 1 << 5;
            cycle = 0;
        }
    }
}