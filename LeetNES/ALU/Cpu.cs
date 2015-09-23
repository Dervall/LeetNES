using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LeetNES.ALU.Instructions;

namespace LeetNES.ALU
{
    public class Cpu : ICpu
    {
        private readonly IMemory mem;
        private int cycle;
        private int scanline = 241;
        private readonly CpuState state;
        private readonly Dictionary<byte, IInstruction> instructions;

        private List<string> instructionLog = new List<string>(); 

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
            File.WriteAllBytes("log.txt", new byte[0]);
        }

        public int Step()
        {
            try
            {
                byte opCode = mem[state.Pc];
                IInstruction instruction;
                if (!instructions.TryGetValue(opCode, out instruction))
                {
                    throw new Exception(string.Format("Unknown instruction {0:X2} encountered at {1:X4}.", opCode, state.Pc));
                }

                LogInstruction(instruction, opCode);

                var instructionTime = instruction.Execute(state, mem);
                cycle += instructionTime * 3;
                if (cycle > 340)
                {
                    scanline++;
                    if (scanline > 260)
                    {
                        scanline = -1;
                    }
                    cycle -= 341;
                }
                return instructionTime;
            }
            catch (Exception e)
            {
                foreach (var logLine in instructionLog)
                {
                    File.AppendAllText("log.txt", logLine);
                }
                throw; 
            }
        }

        private void LogInstruction(IInstruction instruction, byte opCode)
        {
             
            var instructionSize = instruction.Variants[opCode].InstructionSize();
            var instructionBytes = mem.SequenceFrom(state.Pc).Take(instructionSize).ToArray();
            //if (false)
            {
                var contents = String.Format("{0:X4}  {1,-10}{2,-32}A:{3:X2} X:{4:X2} Y:{5:X2} P:{6:X2} SP:{7:X2} CYC:{8,3} SL:{9}\n", // TODO: Cycle and scanline goes at the end
                    state.Pc,
                    string.Join(" ", instructionBytes.Select(x => x.ToString("X2"))),
                    instruction.Disassemble(instructionBytes, mem, state),
                    state.A,
                    state.X,
                    state.Y,
                    state.StatusRegister,
                    state.Sp,
                    cycle,
                    scanline);
                instructionLog.Add(contents);
//                File.AppendAllText("log.txt",
  //                  contents);
            }
        }

        public void Reset()
        {
            state.A = state.X = state.Y = 0;
            // TODO: FOR NESTEST ONLY
            state.Pc = 0xC000; //(ushort) ((mem[0xFFFD] << 8) | mem[0xFFFC]);
            state.StatusRegister = 1 << 5;
            state.SetFlag(CpuState.Flags.InterruptDisable, true);
            state.Sp = 0xFD;
            cycle = 0;
        }

        public void Nmi()
        {
            state.Interrupt(0xFFFA, mem);
            cycle += 7;
        }
    }
}