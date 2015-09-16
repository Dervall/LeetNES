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
            public ushort Pc;
            public byte A;
            public byte X;
            public byte Y;
            public byte StatusRegister;
            public byte Sp;

            public void SetFlag(Flags flag, int i)
            {
                SetFlag(flag, i != 0);                
            }

            public void SetFlag(Flags flag, bool b)
            {
                if (b)
                    StatusRegister |= (byte)flag;
                else
                    StatusRegister &= (byte)~flag;
            }
        }

        private int cycle;
        private readonly State state;
        private readonly Dictionary<byte, IInstruction> instructions;

        public Cpu(IMemory mem, IEnumerable<IInstruction> instructions)
        {
            this.mem = mem;
            state = new State();
            this.instructions = new Dictionary<byte, IInstruction>();
            foreach (var instruction in instructions)
            {
                foreach (var opCode in instruction.Variants.Select(x => x.Key))
                {
                    this.instructions.Add(opCode, instruction);
                }
            }
        }

        public int Step()
        {
            byte opCode = mem[state.Pc];
            IInstruction instruction;
            if (!instructions.TryGetValue(opCode, out instruction))
            {
                throw new Exception("Unknown instruction " + opCode.ToString("X") + " encountered.");
            }

            LogInstruction(instruction, opCode);

            var instructionTime = instruction.Execute(state, mem);
            cycle += instructionTime;
            return instructionTime;
        }

        private void LogInstruction(IInstruction instruction, byte opCode)
        {
            var instructionSize = instruction.Variants[opCode].InstructionSize();
            var instructionBytes = mem.SequenceFrom(state.Pc).Take(instructionSize).ToArray();
            Console.WriteLine("{0:X2}  {1,-10}{2,-32}A:{3:X2} X:{4:X2} Y:{5:X2} P:{6:X2} SP:{7:X2}", // TODO: Cycle and scanline goes at the end
                state.Pc,
                string.Join(" ", instructionBytes.Select(x => x.ToString("X2"))),
                instruction.Disassemble(instructionBytes),
                state.A,
                state.X,
                state.Y,
                state.StatusRegister,
                state.Sp);
        }

        public void Reset()
        {
            state.A = state.X = state.Y = state.Sp = 0;
            state.Pc = (ushort) ((mem[0xFFFD] << 8) | mem[0xFFFC]);
            state.StatusRegister = 1 << 5;
            cycle = 0;
        }
    }
}