using System;
using System.Collections.Generic;
using System.Linq;
using LeetNES.ALU.Instructions;

namespace LeetNES.ALU
{
    public class Cpu : ICpu
    {
        private readonly IMemory mem;
        private int cycle;
        private readonly CpuState state;
        private readonly Dictionary<byte, IInstruction> instructions;

        public Cpu(IMemory mem, IEnumerable<IInstruction> instructions)
        {
            this.mem = mem;
            state = new CpuState();
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
                throw new Exception(string.Format("Unknown instruction {0:X2} encountered at {1:X4}.", opCode, state.Pc));
            }

            LogInstruction(instruction, opCode);

            var instructionTime = instruction.Execute(state, mem);
            cycle += instructionTime;
            return instructionTime;
        }

        private void LogInstruction(IInstruction instruction, byte opCode)
        {
             /*
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
                state.Sp);*/
        }

        public void Reset()
        {
            state.A = state.X = state.Y = state.Sp = 0;
            state.Pc = (ushort) ((mem[0xFFFD] << 8) | mem[0xFFFC]);
            state.StatusRegister = 1 << 5;
            cycle = 0;
        }

        public void Nmi()
        {
            state.Interrupt(0xFFFA, mem);
            cycle += 7;
        }
    }
}