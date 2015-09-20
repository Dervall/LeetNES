namespace LeetNES.ALU
{
    public class CpuState
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

        public ushort Pc;
        public byte A;
        public byte X;
        public byte Y;
        public byte StatusRegister;
        public byte Sp;

        public void SetZeroFlag(byte b)
        {
            SetFlag(Flags.Zero, b == 0);
        }

        public void SetNegativeFlag(byte b)
        {
            SetFlag(Flags.Negative, (b & 0x80) != 0);
        }

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

        public bool IsFlagSet(Flags flag)
        {
            return (StatusRegister & (byte)flag) != 0;
        }

        public void PushStack(byte b, IMemory memory)
        {
            memory[Sp--] = b;
        }

        public byte PopStack(IMemory memory)
        {
            return memory[Sp++];
        }

        public void Interrupt(int interruptAddress, IMemory mem)
        {
            PushStack((byte)(Pc >> 8), mem);
            PushStack((byte)(Pc & 0xFF), mem);
            PushStack(StatusRegister, mem);
            SetFlag(Flags.InterruptDisable, true);
            Pc = mem.ReadShort(interruptAddress);
        }
    }
}