using System;
using System.Drawing;
using System.Windows.Forms;
using LeetNES;

namespace LeetNESEmulator
{
    public partial class MainForm : Form, IPresenter
    {
        private readonly Bitmap _surface = new Bitmap(257, 241);

        public MainForm()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint,
                true);
        }

        public void SetPixel(int pixelX, int pixelY, uint c)
        {
            // Slow as a dog, but fix later.
            var color = Color.FromArgb((int) ((c  >> 24) & 0xFF), (int) ((c >> 16) & 0xFF), (int) ((c >> 8) & 0xFF)); 
            _surface.SetPixel(pixelX, pixelY, color);
        }

        public void Flip()
        {
            Invoke(new Action(Invalidate));
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(
                _surface,
                ClientRectangle,
                new Rectangle(0, 0, _surface.Width, _surface.Height),
                GraphicsUnit.Pixel);
        }
    }
}
