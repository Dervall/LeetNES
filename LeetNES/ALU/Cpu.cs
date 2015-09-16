using System;
using System.Collections.Generic;
using System.Linq;
using LeetNES.ALU.Instructions;

namespace LeetNES.ALU
{
    public class Cpu : ICpu
    {
        public enum Flags
        {
            Carry = 1,
            Zero = 1 << 1,
            InterruptDisable = 1 << 2,
            DecimalMode = 1 << 3,
            Break = 1 << 4,
            Overflow = 1 << 6,
            Negative = 1 << 7
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