using System;

namespace LeetNES.ALU.Instructions
{
    /*
    BVC  Branch on Overflow Clear

     branch on V = 0                  N Z C I D V
                                      - - - - - -

     addressing    assembler    opc  bytes  cyles
     --------------------------------------------
     relative      BVC oper      50    2     2**
    */
    public class BVC : BaseBranchInstruction
    {
        public override string Mnemonic
        {
            get { return "BVC"; }
        }

        protected override byte OpCode
        {
            get { return 0x50; }
        }

        protected override bool ShouldBranch(CpuState cpuState, IMemory memory)
        {
            return !cpuState.IsFlagSet(CpuState.Flags.Overflow);
        }
    }
}