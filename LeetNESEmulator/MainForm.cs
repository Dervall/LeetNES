using System.Drawing;
using System.Windows.Forms;

namespace LeetNESEmulator
{
    public partial class MainForm : Form, IPresenter
    {
        private readonly Bitmap _surface = new Bitmap(256, 240);

        public MainForm()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint,
                true);
        }

        public void SetPixel(int pixelX, int pixelY, int paletteIndex)
        {
            // Slow as a dog, but fix later.
            _surface.SetPixel(pixelX, pixelY, Color.Red);
            Invalidate(new Rectangle(pixelX, pixelY, 1, 1));
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(_surface, 0, 0);
        }
    }
}
