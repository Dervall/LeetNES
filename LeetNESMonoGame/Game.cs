using System.Diagnostics;
using LeetNES;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LeetNESMonoGame
{
    public class MyGame : Game, IPresenter, IIO
    {
        private readonly IEmulator emulator;
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private readonly uint[] pixelBuffer = new uint[256 * 240];
        private bool frameReady = false;
        private int frameCount = 0;
        private int[] controllerReadCounter = new int[2];
        private bool controllerStrobing;
        private KeyboardState keyboardState;

        public MyGame(IEmulator emulator)
        {
            this.emulator = emulator;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            var cartridge = new Cartridge("../../../LeetNES/roms/nestest.nes");//"Bomberman (U).nes");
            emulator.LoadCartridge(cartridge);
            emulator.Reset();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = new Texture2D(graphics.GraphicsDevice, 256, 240, false, SurfaceFormat.Color);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            while (!frameReady)
            {
                emulator.Step();
            }
            

            base.Update(gameTime);
            frameReady = false;
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(0, 0, 256, 240), Color.White);

            spriteBatch.End();

            Window.Title = "LeetNES " + (frameCount / (gameTime.TotalGameTime.TotalMilliseconds/1000.0));

            base.Draw(gameTime);
        }

        public void SetPixel(int pixelX, int pixelY, uint c)
        {
            pixelBuffer[(256*pixelY) + pixelX] = Color.FromNonPremultiplied((int)((c >> 24) & 0xFF), (int)((c >> 16) & 0xFF), (int)((c >> 8) & 0xFF), 255).PackedValue;;//(arbg << 8) | (0xFF000000) ;
        }

        public void Flip()
        {
            frameCount++;
            texture.SetData(pixelBuffer);
            
            frameReady = true;
        }

        public void SetStrobe(bool b)
        {
            controllerStrobing = b;
            controllerReadCounter[0] = controllerReadCounter[1] = 0;
        }

        public byte ReadController(int controller)
        {
            
            var button = (IOReadOrder)controllerReadCounter[controller];
            if (controllerStrobing == false)
            {
                controllerReadCounter[controller]++;
                controllerReadCounter[controller] %= 8;
            }

            switch (button)
            {
                case IOReadOrder.A:
                    return keyboardState.IsKeyDown(Keys.A)  ? (byte)1 : (byte)0;
                case IOReadOrder.B:
                    return keyboardState.IsKeyDown(Keys.S) ? (byte)1 : (byte)0;
                case IOReadOrder.Select:
                    return keyboardState.IsKeyDown(Keys.RightShift) ? (byte) 1 : (byte) 0;
                case IOReadOrder.Start:
                    return keyboardState.IsKeyDown(Keys.Enter) ? (byte)1 : (byte)0;
                case IOReadOrder.Up:
                    return keyboardState.IsKeyDown(Keys.Up) ? (byte)1 : (byte)0;
                case IOReadOrder.Down:
                    var isKeyDown = keyboardState.IsKeyDown(Keys.Down);
                    return isKeyDown ? (byte)1 : (byte)0;
                case IOReadOrder.Left:
                    return keyboardState.IsKeyDown(Keys.Left) ? (byte)1 : (byte)0;
                case IOReadOrder.Right:
                default:
                    return keyboardState.IsKeyDown(Keys.Right) ? (byte)1 : (byte)0;
            }
        }

    }
}
