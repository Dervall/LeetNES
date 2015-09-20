namespace LeetNES
{
    public interface IPresenter
    {
        void SetPixel(int pixelX, int pixelY, uint arbg);
        void Flip();
    }
}
