namespace LeetNES.ALU
{
    public interface ICpu
    {
        int Step();
        void Reset();
        void Nmi();
    }
}