namespace LeetNES.ALU.Instructions
{
    /// <summary>
    /// BPL  Branch on Result Plus
    ///
    /// branch on N = 0                  N Z C I D V
    ///                                  - - - - - -
    ///
    /// addressing    assembler    opc  bytes  cyles
    /// --------------------------------------------
    /// relative      BPL oper      10    2     2**
    /// </summary>
    public class BPL : BaseBranchInstruction
    {
        public override string Mnemonic
        {
            get { return "BPL"; }
        }

        protected override byte OpCode
        {
            get { return 0x10; }
        }

        protected override bool ShouldBranch(Cpu.State cpuState, IMemory memory)
        {
            return !cpuState.IsFlagSet(Cpu.Flags.Negative);
        }
    }
}