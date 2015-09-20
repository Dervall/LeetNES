namespace LeetNES.ALU.Instructions
{
    //BCC  Branch on Carry Clear

    // branch on C = 0                  N Z C I D V
    //                                  - - - - - -

    // addressing    assembler    opc  bytes  cyles
    // --------------------------------------------
    // relative      BCC oper      90    2     2**
    public class BCC : BaseBranchInstruction
    {
        public override string Mnemonic
        {
            get { return "BCC"; }
        }

        protected override byte OpCode
        {
            get { return 0x90; }
        }

        protected override bool ShouldBranch(CpuState cpuState, IMemory memory)
        {
            return !cpuState.IsFlagSet(CpuState.Flags.Carry);
        }
    }
}