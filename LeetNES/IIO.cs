namespace LeetNES
{
    public interface IIO
    {
        void SetStrobe(bool b);
        byte ReadController(int controller);
    }

    public enum IOReadOrder
    {
        A, B, Select, Start, Up, Down, Left, Right
    }
}