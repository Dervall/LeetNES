namespace LeetNES.ALU.Instructions
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
}